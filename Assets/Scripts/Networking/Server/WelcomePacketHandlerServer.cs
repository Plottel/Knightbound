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

            case WelcomeMessage.RequestSpawn:
                {
                    int newPlayerID = reader.ReadInt32();
                    PlayerInfo newPlayerInfo = pms.InitialSpawnPlayer(newPlayerID);
                    PlayerInfo[] playerList = pms.GetPlayerList();

                    rms.RegisterPlayer(newPlayerID);

                    // Don't duplicate Server objects on Local Client
                    if (newPlayerID != 0)
                    {
                        rms.SendFullSync(newPlayerID);
                        VoxelManagerServer.Get.SendVoxelData(newPlayerID);
                    }

                    // Tell new player about all Players (including themselves)
                    foreach (PlayerInfo playerInfo in playerList)
                    {
                        var playerInfoPacket = PacketHelperServer.MakeSetPlayerInfoPacket(playerInfo.playerID, playerInfo.characterID);
                        nms.SendPacket(newPlayerID, playerInfoPacket);
                    }

                    // Tell all OTHER Players about new player
                    var newPlayerInfoPacket = PacketHelperServer.MakeSetPlayerInfoPacket(newPlayerID, newPlayerInfo.characterID);
                    nms.BroadcastPacket(newPlayerInfoPacket, newPlayerID);

                    // Approve spawn for new player
                    var spawnApprovedPacket = PacketHelperServer.MakeSpawnApprovedPacket();
                    nms.SendPacket(newPlayerID, spawnApprovedPacket);            
                }
                break;

            case WelcomeMessage.RequestBeginPlaying:
                {
                    int playerID = reader.ReadInt32();

                    // Fires eventPlayerJoined
                    pms.FinalizePlayerWelcome(playerID);

                    var packet = PacketHelperServer.MakeBeginPlayingApprovedPacket();
                    nms.SendPacket(playerID, packet);
                }
                break;
        }

        Debug.Log("SERVER RECEIVES WELCOME. MESSAGE: " + message.ToString());
    }
}
