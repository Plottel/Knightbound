using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Deft;

public class NetworkManagerClient : Manager<NetworkManagerClient>
{
    public NetworkState state;
    public int playerID;
    public int playerObjNetworkID;
    public NetworkReplicator replicator;

    private GameClient client;
    private PlayerControllerClient controller;

    private Dictionary<PacketType, PacketHandlerClient> packetHandlers;


    public override void OnAwake()
    {
        base.OnAwake();

        replicator = new NetworkReplicator();

        client = new GameClient();

        packetHandlers = new Dictionary<PacketType, PacketHandlerClient>();
        SetPacketHandler<WelcomePacketHandlerClient>(PacketType.Welcome);
        SetPacketHandler<ConsoleMessagePacketHandlerClient>(PacketType.ConsoleMessage);
        SetPacketHandler<ReplicationPacketHandlerClient>(PacketType.Replication);

        controller = new PlayerControllerClient();
    }

    public void SetContext(NetworkContext context)
    {
        replicator.context = context;
    }

    public void SetPlayerID(int playerID)
    {
        this.playerID = playerID;
    }

    public void SetPlayerObjNetworkID(int playerObjNetworkID)
    {
        this.playerObjNetworkID = playerObjNetworkID;

        if (replicator.context.TryGetNetworkObject(playerObjNetworkID, out var obj))
            controller.player = obj as Tree;
    }

    public void SetPacketHandler<T>(PacketType packetType) where T : PacketHandlerClient
    {
        T packetHandler = Activator.CreateInstance<T>();
        packetHandler.client = client;

        packetHandlers[packetType] = packetHandler;
    }

    public void JoinServer(string hostName, ushort port)
    {
        state = NetworkState.Connecting;
        client.JoinServer(hostName, port, OnServerConnectionEstablished);
    }

    // This is actually gameplay layer.
    // Begin thinking on how to factor out the NMC and NMS
    private void OnServerConnectionEstablished()
    {
        state = NetworkState.Connected;
        SendWelcomePacket(WelcomeState.PlayerID);
    }

    // Called when server connection is established.
    public void SendWelcomePacket(WelcomeState state)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                writer.Write((int)PacketType.Welcome);
                writer.Write((int)state);
            }

            client.SendPacket(stream);
        }
    }

    public override void OnUpdate()
    {
        ProcessIncomingPackets();

        // How to map Player ID to the Player Object network ID
        // Welcome packet needs to send that network ID i think.
        if (state == NetworkState.Welcomed)
            controller.OnUpdate();
    }

    public void ProcessIncomingPackets()
    {
        if (state == NetworkState.Uninitialized)
            return;

        using (MemoryStream stream = new MemoryStream())
        {
            while (client.PumpPacket(stream))
            {
                stream.Position = 0;

                // Memory Stream containing PacketData
                using (BinaryReader reader = new BinaryReader(stream, Encoding.Default, true))
                {
                    string ip = reader.ReadString();
                    PacketType packetType = (PacketType)reader.ReadInt32();

                    HandlePacket(ip, packetType, reader);
                }
            }
        }
    }

    private void HandlePacket(string ip, PacketType packetType, BinaryReader reader)
    {
        Debug.Log("CLIENT: Packet Received. Type: " + packetType.ToString());
        packetHandlers[packetType].HandlePacket(ip, reader);
    }

    public void SendPacket(MemoryStream stream)
    {
        client.SendPacket(stream);
    }

    public void SendInputPacket(InputState inputState)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                writer.Write((int)PacketType.Input);
                writer.Write(playerID);
                inputState.Serialize(writer);
            }

            client.SendPacket(stream);
        }
    }

    public void SendConsoleMessagePacket(string message)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                writer.Write((int)PacketType.ConsoleMessage);
                writer.Write(message);
            }

            client.SendPacket(stream);
        }
    }
}
