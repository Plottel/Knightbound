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
    public delegate void PlayerJoinedHandler(PlayerInfo clientInfo);
    public event PlayerJoinedHandler eventPlayerJoined;

    private NetworkServer server;

    private int nextPlayerID;

    private Dictionary<uint, ClientInfo> peerIDToClientInfo;
    private Dictionary<int, ClientInfo> playerIDToClientInfo;

    private Dictionary<int, PlayerInfo> playerIDToPlayerInfo;
    private Dictionary<uint, PlayerInfo> peerIDToPlayerInfo;

    private Dictionary<PacketType, PacketHandlerServer> packetHandlers;

    public override void OnAwake()
    {
        base.OnAwake();

        server = new NetworkServer();

        nextPlayerID = 0;

        peerIDToClientInfo = new Dictionary<uint, ClientInfo>();
        playerIDToClientInfo = new Dictionary<int, ClientInfo>();

        playerIDToPlayerInfo = new Dictionary<int, PlayerInfo>();
        peerIDToPlayerInfo = new Dictionary<uint, PlayerInfo>();

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
    }

    public ClientInfo RegisterNewClient(uint peerID)
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

    public void RegisterNewPlayer(ClientInfo client, int networkID)
    {
        var playerInfo = new PlayerInfo
        {
            clientInfo = client,
            networkID = networkID
        };

        peerIDToPlayerInfo.Add(client.peerID, playerInfo);
        playerIDToPlayerInfo.Add(client.playerID, playerInfo);
    }

    public void WelcomeRegisteredPlayer(PlayerInfo player)
    {
        eventPlayerJoined?.Invoke(player);
    }

    public bool GetClientInfo(int playerID, out ClientInfo clientInfo)
    {
        return playerIDToClientInfo.TryGetValue(playerID, out clientInfo);
    }

    public bool GetClientInfo(uint peerID, out ClientInfo clientInfo)
    {
        return peerIDToClientInfo.TryGetValue(peerID, out clientInfo);
    }

    public bool GetPlayerInfo(int playerID, out PlayerInfo playerInfo)
    {
        return playerIDToPlayerInfo.TryGetValue(playerID, out playerInfo);
    }

    public bool GetPlayerInfo(uint peerID, out PlayerInfo playerInfo)
    {
        return peerIDToPlayerInfo.TryGetValue(peerID, out playerInfo);
    }

    public bool HasClient(int playerID)
    {
        return playerIDToClientInfo.ContainsKey(playerID);
    }

    public bool HasClient(uint peerID)
    {
        return peerIDToClientInfo.ContainsKey(peerID);
    }

    public override void OnUpdate()
    {
        HandleIncomingPackets();
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

    public void SendPacket(uint peerID, MemoryStream stream)
    {
        server.SendPacket(peerID, stream);
    }

    public void TrueBroadcastPacket(MemoryStream stream)
    {
        server.TrueBroadcastPacket(stream);
    }
}