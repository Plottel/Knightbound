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
    private Dictionary<int, ClientProxy> clientProxies;

    private Dictionary<PacketType, PacketHandlerServer> packetHandlers;

    public NetworkReplicator replicator;

    private const int kUpdateSendInterval = 1;
    private float timeSinceLastUpdate;

    public override void OnAwake()
    {
        base.OnAwake();

        server = new NetworkServer();

        nextPlayerID = 0;
        clientProxies = new Dictionary<int, ClientProxy>();

        packetHandlers = new Dictionary<PacketType, PacketHandlerServer>();

        SetPacketHandler<WelcomePacketHandlerServer>(PacketType.Welcome);
        SetPacketHandler<ConsoleMessagePacketHandlerServer>(PacketType.ConsoleMessage);
        SetPacketHandler<InputPacketHandlerServer>(PacketType.Input);

        replicator = new NetworkReplicator();
    }

    public bool HasPeerID(uint peerID)
    {
        foreach (var player in clientProxies.Values)
        {
            if (player.peerID == peerID)
                return true;
        }

        return false;
    }

    public bool HasPlayerID(int playerID)
    {
        return clientProxies.ContainsKey(playerID);
    }

    public bool GetPlayerID(uint peerID, out int playerID)
    {
        foreach (var clientProxy in GetClientProxies())
        {
            if (clientProxy.peerID == peerID)
            {
                playerID = clientProxy.playerID;
                return true;
            }
        }

        playerID = -1;
        return false;
    }

    public ClientProxy GetClientProxy(int playerID)
    {
        return clientProxies[playerID];
    }

    public ClientProxy[] GetClientProxies()
    {
        return clientProxies.Values.ToArray();
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

    public ClientProxy RegisterNewPlayer(uint peerID)
    {
        int playerID = nextPlayerID++;

        var proxy = new ClientProxy();
        proxy.state = WelcomeState.Uninitialized;
        proxy.peerID = peerID;
        proxy.playerID = playerID;

        clientProxies.Add(playerID, proxy);
        return proxy;
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
                if (replicator.context.
                TryGetNetworkObject(clientProxy.playerObjNetworkID, out var playerObj))
                {
                    if (inputState.move)
                        playerObj.transform.position += new Vector3(1f, 0, 0);
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

        if (Input.GetKeyDown(KeyCode.U))
        {
            SendCreateAllObjects(1);
        }
    }

    private void SynchronizeClientProxyReplicators()
    {
        var networkObjects = GetNetworkObjects();

        foreach (ClientProxy client in clientProxies.Values)
        {
            foreach (NetworkObject obj in networkObjects)
            {
                if (client.replicator.context.TryGetNetworkID(obj, out int networkID))
                {
                    SendUpdate(client.playerID, networkID, obj);
                }
                else
                {
                    client.replicator.context.RegisterNetworkObject(networkID, obj);
                    SendCreate(client.playerID, networkID, obj);
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

    public void SendPacket(int playerID, MemoryStream stream)
    {
        server.SendPacket(clientProxies[playerID].peerID, stream);
    }

    public void SendWelcome(WelcomeState state, int playerID, int playerObjNetworkID)
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

            server.SendPacket(clientProxies[playerID].peerID, stream);
        }
    }

    private void SendCreate(int playerID, int networkID, NetworkObject obj)
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

            server.SendPacket(clientProxies[playerID].peerID, stream);
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

            server.SendPacket(clientProxies[playerID].peerID, stream);
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
