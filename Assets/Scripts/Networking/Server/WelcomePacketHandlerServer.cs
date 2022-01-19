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
        PlayerManagerServer pms = PlayerManagerServer.Get;
        ReplicationManagerServer rms = ReplicationManagerServer.Get;
        var message = (WelcomeMessage)reader.ReadInt32();

        switch (message)
        {
            // Respond to New Players with their Player ID
            case WelcomeMessage.RequestConnection:
                {
                    if (!nms.HasPeer(peerID))
                    {
                        int playerID = nms.RegisterNewPeer(peerID);
                        pms.RegisterNewPlayer(playerID);

                        var packet = PacketHelperServer.MakeConnectionApprovedPacket(playerID);
                        nms.SendPacket(playerID, packet);
                    }
                }
                break;

            // Create Player object and send ID to Client
            case WelcomeMessage.RequestSpawn:
                {
                    int playerID = reader.ReadInt32();
                    PlayerInfo playerInfo = pms.SpawnPlayer(playerID);

                    var playerInfoPacket = PacketHelperServer.MakeSetPlayerInfoPacket(playerID, playerInfo.characterID);
                    var spawnApprovedPacket = PacketHelperServer.MakeSpawnApprovedPacket();

                    nms.TrueBroadcastPacket(playerInfoPacket);
                    nms.SendPacket(playerID, spawnApprovedPacket);
                }
                break;

            // Upgrade Client to Player and it becomes live!
            case WelcomeMessage.RequestBeginPlaying:
                {
                    int playerID = reader.ReadInt32();
                    pms.FinalizePlayerWelcome(playerID);

                    var packet = PacketHelperServer.MakeBeginPlayingApprovedPacket();
                    nms.SendPacket(playerID, packet);
                }
                break;
        }

        Debug.Log("SERVER RECEIVES WELCOME. MESSAGE: " + message.ToString());
    }
}
