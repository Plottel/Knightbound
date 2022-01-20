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
    private Dictionary<PacketType, PacketHandlerClient> packetHandlers;

    public override void OnAwake()
    {
        base.OnAwake();

        state = NetworkState.Uninitialized;
        playerID = -1;
        client = new NetworkClient();
        packetHandlers = new Dictionary<PacketType, PacketHandlerClient>();

        SetPacketHandler<WelcomePacketHandlerClient>(PacketType.Welcome);
        SetPacketHandler<ConsoleMessagePacketHandlerClient>(PacketType.ConsoleMessage);
        SetPacketHandler<ReplicationPacketHandlerClient>(PacketType.Replication);
        SetPacketHandler<SetPlayerInfoPacketHandlerClient>(PacketType.SetPlayerInfo);
    }

    public override void OnUpdate()
    {
        ProcessIncomingPackets();
    }

    public void ProcessIncomingPackets()
    {
        if (state == NetworkState.Uninitialized)
            return;

        using (MemoryStream stream = new MemoryStream())
        {
            long streamPosition = stream.Position;

            while (client.PumpPacket(stream))
            {
                // We're writing to the Memory Stream.
                // When it finishes writing, its position will be at the end.
                // We want to reset it to where it was before it started writing,
                // so that we can read what it just wrote.
                long tempStreamPosition = stream.Position;
                stream.Position = streamPosition;
                streamPosition = tempStreamPosition;

                // Memory Stream containing PacketData
                using (BinaryReader reader = new BinaryReader(stream, Encoding.Default, true))
                {
                    string originIP = reader.ReadString();
                    PacketType packetType = (PacketType)reader.ReadInt32();

                    HandlePacket(originIP, packetType, reader);
                }
            }
        }
    }

    private void HandlePacket(string originIP, PacketType packetType, BinaryReader reader)
    {
        Debug.Log("CLIENT RECEIVES: " + packetType.ToString());
        packetHandlers[packetType].HandlePacket(originIP, reader);
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
        SendPacket(PacketHelperClient.MakeRequestConnectionPacket());
    }

    public void SetPacketHandler<T>(PacketType packetType) where T : PacketHandlerClient
    {
        T packetHandler = Activator.CreateInstance<T>();
        packetHandlers[packetType] = packetHandler;
    }
}
