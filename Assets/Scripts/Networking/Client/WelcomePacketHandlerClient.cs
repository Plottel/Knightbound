using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Deft.Networking;

public class WelcomePacketHandlerClient : PacketHandlerClient
{
    public override void HandlePacket(string originIP, BinaryReader reader)
    {
        var nmc = NetworkManagerClient.Get;
        var message = (WelcomeMessage)reader.ReadInt32();

        switch (message)
        {
            // Set Player ID and Request Spawn from server.
            case WelcomeMessage.ConnectionApproved:
                {
                    int playerID = reader.ReadInt32();

                    nmc.state = NetworkState.Connected;
                    nmc.playerID = playerID;

                    var packet = PacketHelperClient.MakeWelcomePacket(WelcomeMessage.RequestSpawn, playerID);
                    nmc.SendPacket(packet);
                }
                break;

            // Set Player Network ID and Request Start
            case WelcomeMessage.SpawnApproved:
                {
                    nmc.state = NetworkState.Welcomed;

                    int playerID = nmc.playerID;
                    var packet = PacketHelperClient.MakeWelcomePacket(WelcomeMessage.RequestBeginPlaying, playerID);
                    nmc.SendPacket(packet);
                }
                break;

            // Ready to Play!
            case WelcomeMessage.BeginPlayingApproved:
                {
                    // Begin Playing! ....
                    nmc.state = NetworkState.Playing;
                }
                break;
        }

        Debug.Log("CLIENT RECEIVES WELCOME. MESSAGE: " + message.ToString());
    }
}
