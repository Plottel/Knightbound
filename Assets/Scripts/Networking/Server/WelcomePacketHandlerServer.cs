using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Deft.Networking;

public class WelcomePacketHandlerServer : PacketHandlerServer
{
    public override void HandlePacket(uint peerID, BinaryReader reader)
    {
        NetworkManagerServer nms = NetworkManagerServer.Get;
        ReplicationManagerServer rms = ReplicationManagerServer.Get;
        var message = (WelcomeMessage)reader.ReadInt32();

        switch (message)
        {
            // Respond to New Players with their Player ID
            case WelcomeMessage.AttemptConnection:
                {
                    if (!nms.HasClient(peerID))
                    {
                        var clientInfo = nms.RegisterNewClient(peerID);
                        var packet = PacketHelperServer.MakeConnectionApprovedPacket(clientInfo.playerID);

                        nms.SendPacket(clientInfo.playerID, packet);
                    }
                }
                break;

            // Create Player object and send ID to Client
            case WelcomeMessage.RequestSpawn:
                {
                    if (nms.GetClientInfo(peerID, out ClientInfo clientInfo))
                    {
                        int networkID;
                        var player = rms.CreateNetworkObject<Player>((int)NetworkObjectType.Player, out networkID);
                        nms.RegisterNewPlayer(clientInfo, networkID);

                        var packet = PacketHelperServer.MakeSpawnPacket(clientInfo.playerID, networkID);
                        nms.SendPacket(clientInfo.playerID, packet);
                    }
                }
                break;

            // Upgrade Client to Player and it becomes live!
            case WelcomeMessage.RequestBeginPlaying:
                {
                    if (nms.GetPlayerInfo(peerID, out PlayerInfo playerInfo))
                    {
                        nms.WelcomeRegisteredPlayer(playerInfo);

                        var packet = PacketHelperServer.MakeBeginPlayingPacket();
                        nms.SendPacket(playerInfo.clientInfo.playerID, packet);
                    }
                }
                break;
        }

        Debug.Log("SERVER RECEIVES WELCOME. MESSAGE: " + message.ToString());
    }
}
