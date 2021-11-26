using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameServer
{
    public NetworkReplicator replicator;

    private ENetServer server;
    private Dictionary<int, string> players;
    private Dictionary<PacketType, PacketHandlerServer> packetHandlers;

    private int nextPlayerID;

    public GameServer()
    {
        replicator = new NetworkReplicator();

        nextPlayerID = 0;
        server = new ENetServer();
        players = new Dictionary<int, string>();
        packetHandlers = new Dictionary<PacketType, PacketHandlerServer>();
    }

    public bool HasPlayer(int playerID) => players.ContainsKey(playerID);
    public bool HasIP(string ip) => players.ContainsValue(ip);

    public void SetPacketHandler<T>(PacketType packetType) where T : PacketHandlerServer
    {
        T packetHandler = Activator.CreateInstance<T>();
        packetHandler.server = this;

        packetHandlers[packetType] = packetHandler;
    }

    public void LaunchServer(ushort port)
    {
        server.LaunchServer(port);
    }

    public int RegisterNewPlayer(string ip)
    {
        int playerID = nextPlayerID++;
        players.Add(playerID, ip);

        return playerID;
    }

    public void ProcessPackets()
    {
        using (MemoryStream stream = new MemoryStream())
        {
            while (server.PumpPacket(stream))
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
        packetHandlers[packetType].HandlePacket(ip, reader);
    }

    public void SendPacket(int playerID, PacketType packetType, GamePacket packet)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(Convert.ToInt32(packetType));
                packet.SerializeHeader(writer);
                packet.SerializeBody(writer);
                server.SendPacket(players[playerID], stream);
            }
        }
    }

    public void BroadcastPacket(PacketType packetType, GamePacket packet)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(Convert.ToInt32(packetType));
                packet.SerializeHeader(writer);
                packet.SerializeBody(writer);
                server.BroadcastPacket(stream);
            }
        }
    }
}
