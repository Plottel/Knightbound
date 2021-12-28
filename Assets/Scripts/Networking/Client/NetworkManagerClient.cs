using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using Deft;
using Deft.Networking;

public class NetworkManagerClient : Manager<NetworkManagerClient>
{
    public NetworkState state;
    public int playerID;

    protected NetworkClient client;
    public NetworkReplicator replicator;
    private Dictionary<PacketType, PacketHandlerClient> packetHandlers;

    private PlayerControllerClient controller;
    public int playerObjNetworkID;

    public override void OnAwake()
    {
        base.OnAwake();

        client = new NetworkClient();
        replicator = new NetworkReplicator();
        packetHandlers = new Dictionary<PacketType, PacketHandlerClient>();

        SetPacketHandler<WelcomePacketHandlerClient>(PacketType.Welcome);
        SetPacketHandler<ConsoleMessagePacketHandlerClient>(PacketType.ConsoleMessage);
        SetPacketHandler<ReplicationPacketHandlerClient>(PacketType.Replication);

        controller = new PlayerControllerClient();
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
        stream.Dispose();
    }

    public void JoinServer(string hostName, ushort port)
    {
        state = NetworkState.Connecting;
        client.JoinServer(hostName, port, OnServerConnectionEstablished);
    }

    private void OnServerConnectionEstablished()
    {
        state = NetworkState.Connected;
        SendPacket(PacketHelperClient.MakeWelcomePacket(WelcomeState.PlayerID));
    }

    public void SetPacketHandler<T>(PacketType packetType) where T : PacketHandlerClient
    {
        T packetHandler = Activator.CreateInstance<T>();
        packetHandlers[packetType] = packetHandler;
    }

    public void SetContext(NetworkContext context)
    {
        replicator.context = context;
    }

    public void SetPlayerID(int playerID)
    {
        this.playerID = playerID;
        controller.playerID = playerID;
    }
    
    public void SetPlayerObjNetworkID(int playerObjNetworkID)
    {
        this.playerObjNetworkID = playerObjNetworkID;

        if (replicator.context.TryGetNetworkObject(playerObjNetworkID, out var obj))
            controller.player = obj as Tree;
    }
}
