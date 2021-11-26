using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameClient
{
    public int playerID;
    public NetworkReplicator replicator;

    private GameClientState state;
    private ENetClient client;
    private Dictionary<PacketType, PacketHandlerClient> packetHandlers;

    public GameClientState State { get => state; set => state = value; }

    public GameClient()
    {
        playerID = -1;
        replicator = new NetworkReplicator();
        client = new ENetClient();
        packetHandlers = new Dictionary<PacketType, PacketHandlerClient>();
    }

    public void SetPacketHandler<T>(PacketType packetType) where T : PacketHandlerClient
    {
        T packetHandler = Activator.CreateInstance<T>();
        packetHandler.client = this;

        packetHandlers[packetType] = packetHandler;
    }

    public void JoinServer(string hostName, ushort port)
    {
        state = GameClientState.Connecting;
        client.JoinServer(hostName, port, OnServerConnectionEstablished);        
    }

    private void OnServerConnectionEstablished()
    {
        var packet = NetworkUtils.CreateWelcomePacket("HELLO");
        SendPacket(PacketType.Welcome, packet);
    }

    public void ProcessPackets()
    {
        if (state == GameClientState.Uninitialized)
            return;

        using (MemoryStream stream = new MemoryStream())
        {
            while (client.PumpPacket(stream))
            {
                stream.Position = 0;

                // Memory Stream containing PacketData
                using (BinaryReader reader = new BinaryReader(stream))
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

    public void SendPacket(PacketType packetType, GamePacket packet)
    {
        if (state == GameClientState.Uninitialized)
            return;

        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(Convert.ToInt32(packetType));
                packet.SerializeHeader(writer);
                packet.SerializeBody(writer);
                client.SendPacket(stream);
            }
        }
    }
}
