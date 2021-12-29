using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Deft;
using Deft.Networking;

public class NetworkManagerServer : Manager<NetworkManagerServer>
{
    private NetworkServer server;

    private int nextPlayerID;
    private Dictionary<uint, ClientInfo> peerIDToClientInfo;
    private Dictionary<int, ClientInfo> playerIDToClientInfo;
    private Dictionary<int, int> playerIDToPlayerNetworkID;
    private Dictionary<int, ClientProxy> playerIDToClientProxy;

    private Dictionary<PacketType, PacketHandlerServer> packetHandlers;

    public NetworkReplicator replicator;

    private const float kUpdateSendInterval = 0.033f;
    private float timeSinceLastUpdate;

    public override void OnAwake()
    {
        base.OnAwake();

        server = new NetworkServer();

        nextPlayerID = 0;
        peerIDToClientInfo = new Dictionary<uint, ClientInfo>();
        playerIDToClientInfo = new Dictionary<int, ClientInfo>();
        playerIDToPlayerNetworkID = new Dictionary<int, int>();
        playerIDToClientProxy = new Dictionary<int, ClientProxy>();

        packetHandlers = new Dictionary<PacketType, PacketHandlerServer>();

        SetPacketHandler<WelcomePacketHandlerServer>(PacketType.Welcome);
        SetPacketHandler<ConsoleMessagePacketHandlerServer>(PacketType.ConsoleMessage);
        SetPacketHandler<InputPacketHandlerServer>(PacketType.Input);

        replicator = new NetworkReplicator();
    }

    public ClientProxy GetClientProxy(int playerID)
    {
        return playerIDToClientProxy[playerID];
    }

    public ClientProxy[] GetClientProxies()
    {
        return playerIDToClientProxy.Values.ToArray();
    }

    public NetworkObject[] GetNetworkObjects()
    {
        return replicator.context.GetNetworkObjects();
    }

    public NetworkContext GetContext()
    {
        return replicator.context;
    }

    public void SetPacketHandler<T>(PacketType packetType) where T : PacketHandlerServer
    {
        T packetHandler = Activator.CreateInstance<T>();
        packetHandlers[packetType] = packetHandler;
    }

    public void LaunchServer(ushort port)
    {
        server.LaunchServer(port);
    }

    public ClientInfo RegisterNewPlayer(uint peerID)
    {
        var clientInfo = new ClientInfo
        {
            playerID = nextPlayerID++,
            peerID = peerID
        };

        peerIDToClientInfo.Add(clientInfo.peerID, clientInfo);
        playerIDToClientInfo.Add(clientInfo.playerID, clientInfo);
        return clientInfo;
    }

    public void CreateClientProxy(uint peerID, int playerID)
    {
        var clientProxy = new ClientProxy
        {
            peerID = peerID,
            playerID = playerID
        };

        playerIDToClientProxy[playerID] = clientProxy;
    }

    public bool GetClientInfo(int playerID, out ClientInfo clientInfo)
    {
        return playerIDToClientInfo.TryGetValue(playerID, out clientInfo);
    }

    public bool GetClientInfo(uint peerID, out ClientInfo clientInfo)
    {
        return peerIDToClientInfo.TryGetValue(peerID, out clientInfo);
    }

    public bool HasClient(int playerID)
    {
        return playerIDToClientInfo.ContainsKey(playerID);
    }

    public bool HasClient(uint peerID)
    {
        return peerIDToClientInfo.ContainsKey(peerID);
    }

    public void SetPlayerNetworkID(int playerID, int networkID)
    {
        playerIDToPlayerNetworkID[playerID] = networkID;
    }

    public T CreateNetworkObject<T>(int classID, out int networkID) where T : NetworkObject
    {
        var newObj = NetworkPrefabRegistry.Create<T>(classID);
        networkID = replicator.context.RegisterNewNetworkObject(newObj);

        return newObj;
    }

    public void DestroyNetworkObject(int networkID)
    {
        // Destroy the Tree
        //replicator.context.DeregisterNetworkObject(networkID);
        //Destroy(tree.gameObject);

        //BroadcastDestroy(networkID);
    }

    public override void OnUpdate()
    {
        HandleIncomingPackets();

        // Process any Input Packets
        foreach (var clientProxy in GetClientProxies())
        {
            if (clientProxy.GetInputState(out InputState inputState))
            {
                int playerNetworkID = playerIDToPlayerNetworkID[clientProxy.playerID];

                if (replicator.context.TryGetNetworkObject(playerNetworkID, out var playerObj))
                {
                    Player player = playerObj as Player; // Lol so bad.

                    float x = 0;
                    float z = 0;

                    if (inputState.up) z = 1f;
                    if (inputState.down) z = -1f;
                    if (inputState.left) x = -1f;
                    if (inputState.right) x = 1f;

                    player.direction = new Vector3(x, 0, z);
                }
            }
        }
    }

    public override void OnLateUpdate()
    {
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate > kUpdateSendInterval)
        {
            timeSinceLastUpdate = 0f;
            SynchronizeClientProxyReplicators();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SendCreate(1, 0, GetNetworkObjects()[0]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SendCreate(1, 1, GetNetworkObjects()[1]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // The 1st Packet sent each frame is duplicated by the number of packets sent.
            SendCreate(1, 1, GetNetworkObjects()[1]);
            SendCreate(1, 0, GetNetworkObjects()[0]);

            var welcome = PacketHelperClient.MakeWelcomePacket(WelcomeMessage.BeginPlaying);
            SendPacket(1, welcome);
            SendPacket(1, welcome);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            var netObjs = GetNetworkObjects();
            SendCreate(1, 0, netObjs[0]);
            SendCreate(1, 1, netObjs[1]);

            //SendCreateAllObjects(1);
        }
    }

    private void SynchronizeClientProxyReplicators()
    {
        var networkObjects = GetNetworkObjects();

        foreach (ClientProxy client in playerIDToClientProxy.Values)
        {
            // Don't send Replication Data to Local Client.
            if (client.peerID == 0)
                continue;

            foreach (NetworkObject obj in networkObjects)
            {
                // If Client has this Network ID, Update the existing object.
                if (client.replicator.context.TryGetNetworkID(obj, out int networkID))
                {
                    Debug.Log("Sending Update to Player " + client.playerID + " Network ID" + networkID);
                    SendUpdate(client.playerID, networkID, obj);
                }
                // If Client does NOT have this Network ID, Create using existing Network ID.
                else
                {
                    networkID = replicator.context.GetNetworkID(obj);
                    client.replicator.context.RegisterNetworkObject(networkID, obj);

                    SendCreate(client.playerID, networkID, obj);
                }
            }
        }
    }

    public void HandleIncomingPackets()
    {
        using (MemoryStream stream = new MemoryStream())
        {
            while (server.PumpPacket(stream))
            {
                stream.Position = 0;

                // Memory Stream containing PacketData
                using (BinaryReader reader = new BinaryReader(stream, Encoding.Default, true))
                {
                    uint peerID = reader.ReadUInt32();
                    HandlePacket(peerID, reader);
                }
            }
        }
    }

    private void HandlePacket(uint peerID, BinaryReader reader)
    {
        PacketType packetType = (PacketType)reader.ReadInt32();
        Debug.Log("SERVER RECEIVES: " + packetType.ToString());
        packetHandlers[packetType].HandlePacket(peerID, reader);
    }


    public void SendPacket(int playerID, MemoryStream stream)
    {
        server.SendPacket(playerIDToClientInfo[playerID].peerID, stream);
    }

    public void SendWelcome(WelcomeMessage state, int playerID, int playerObjNetworkID)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                writer.Write((int)PacketType.Welcome);
                writer.Write((int)state);
                writer.Write(playerID);
                writer.Write(playerObjNetworkID);
            }

            server.SendPacket(playerIDToClientProxy[playerID].peerID, stream);
        }
    }

    private void SendCreate(int playerID, int networkID, NetworkObject obj)
    {
        Debug.Log("Sending Create - ID " + networkID.ToString());

        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                writer.Write((int)PacketType.Replication);
                writer.Write((int)ReplicationAction.Create);
                writer.Write(obj.GetClassID());
                writer.Write(networkID);
                obj.Serialize(writer);
            }

            server.SendPacket(playerIDToClientProxy[playerID].peerID, stream);
        }
    }

    public void SendCreateAllObjects(int playerID)
    {
        foreach (NetworkObject obj in GetNetworkObjects())
        {
            if (replicator.context.TryGetNetworkID(obj, out int networkID))
                SendCreate(playerID, networkID, obj);
        }
    }

    private void BroadcastCreate(NetworkObject obj, int networkID)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                writer.Write((int)PacketType.Replication);
                writer.Write((int)ReplicationAction.Create);
                writer.Write((int)obj.GetClassID());
                writer.Write(networkID);
                obj.Serialize(writer);
            }

            server.BroadcastPacket(stream);
        }
    }

    private void BroadcastUpdate(int networkID, NetworkObject obj)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                writer.Write((int)PacketType.Replication);
                writer.Write((int)ReplicationAction.Update);
                writer.Write((int)obj.GetClassID());
                writer.Write(networkID);
                obj.Serialize(writer);
            }

            server.BroadcastPacket(stream);
        }
    }

    private void SendUpdate(int playerID, int networkID, NetworkObject obj)
    {
        Debug.Log("Sending Update: ID " + networkID.ToString());

        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                writer.Write((int)PacketType.Replication);
                writer.Write((int)ReplicationAction.Update);
                writer.Write((int)obj.GetClassID());
                writer.Write(networkID);
                obj.Serialize(writer);
            }

            server.SendPacket(playerIDToClientProxy[playerID].peerID, stream);
        }
    }

    private void BroadcastDestroy(int networkID)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                writer.Write((int)PacketType.Replication);
                writer.Write((int)ReplicationAction.Destroy);
                writer.Write(networkID);
                // No need to serialize object data for Destroy.
            }

            server.BroadcastPacket(stream);
        }
    }

    public void TrueBroadcastPacket(MemoryStream stream)
    {
        server.TrueBroadcastPacket(stream);
    }
}
