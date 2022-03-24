using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Deft;
using Deft.Networking;
using Random = UnityEngine.Random;

public class NetworkManagerServer : Manager<NetworkManagerServer>
{
    private NetworkServer server;

    private int nextPlayerID;

    private Dictionary<uint, int> peerIDToPlayerID;
    private Dictionary<int, uint> playerIDToPeerID;

    private Dictionary<PacketType, PacketHandlerServer> packetHandlers;

    public override void OnAwake()
    {
        base.OnAwake();

        server = new NetworkServer();

        nextPlayerID = 0;

        peerIDToPlayerID = new Dictionary<uint, int>();
        playerIDToPeerID = new Dictionary<int, uint>();

        packetHandlers = new Dictionary<PacketType, PacketHandlerServer>();

        SetPacketHandler<WelcomePacketHandlerServer>(PacketType.Welcome);
        SetPacketHandler<ConsoleMessagePacketHandlerServer>(PacketType.ConsoleMessage);
        SetPacketHandler<InputPacketHandlerServer>(PacketType.Input);
    }

    public void SetPacketHandler<T>(PacketType packetType) where T : PacketHandlerServer
    {
        T packetHandler = Activator.CreateInstance<T>();
        packetHandlers[packetType] = packetHandler;
    }

    public void LaunchServer(ushort port)
    {
        server.LaunchServer(port);

        // We know this is a Listen Server. Prevent duplication to Local Client.
        ReplicationManagerClient.Get.SetNetworkContext(ReplicationManagerServer.Get.NetworkContext);

        // TODO: Consider Server Launch event
        int seed = Random.Range(0, int.MaxValue);
        WorldManagerServer.Get.GenerateWorld(seed);
    }

    public int RegisterNewPeer(uint peerID)
    {
        int playerID = nextPlayerID++;

        peerIDToPlayerID.Add(peerID, playerID);
        playerIDToPeerID.Add(playerID, peerID);
        return playerID;
    }

    public bool HasPeer(int playerID)
    {
        return playerIDToPeerID.ContainsKey(playerID);
    }

    public bool HasPeer(uint peerID)
    {
        return peerIDToPlayerID.ContainsKey(peerID);
    }

    public override void OnUpdate()
    {
        if (server.IsActive)
            HandleIncomingPackets();
    }

    public void HandleIncomingPackets()
    {
        int packetCount = 0;

        using (MemoryStream stream = new MemoryStream())
        {
            long streamPosition = stream.Position;

            while (server.PumpPacket(stream))
            {
                ++packetCount;

                long tempStreamPosition = stream.Position;
                stream.Position = streamPosition;
                streamPosition = tempStreamPosition;

                // Memory Stream containing PacketData
                using (BinaryReader reader = new BinaryReader(stream, Encoding.Default, true))
                {
                    uint peerID = reader.ReadUInt32();
                    HandlePacket(peerID, reader);
                }
            }
        }

        Debug.Log("PACKET COUNT: " + packetCount.ToString());
    }

    private void HandlePacket(uint peerID, BinaryReader reader)
    {
        PacketType packetType = (PacketType)reader.ReadInt32();
        Debug.Log("SERVER RECEIVES: " + packetType.ToString());
        packetHandlers[packetType].HandlePacket(peerID, reader);
    }

    public void SendPacket(int playerID, MemoryStream stream)
    {
        server.SendPacket(playerIDToPeerID[playerID], stream);
    }

    public void BroadcastPacket(MemoryStream stream)
    {
        server.BroadcastPacket(stream);
    }

    public void BroadcastPacket(MemoryStream stream, int excludedPlayerID)
    {
        server.BroadcastPacket(stream, playerIDToPeerID[excludedPlayerID]);
    }

    public void TrueBroadcastPacket(MemoryStream stream)
    {
        server.TrueBroadcastPacket(stream);
    }
}