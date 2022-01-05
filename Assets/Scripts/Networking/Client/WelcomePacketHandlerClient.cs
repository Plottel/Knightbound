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
                    nmc.state = NetworkState.Connected;

                    int playerID = reader.ReadInt32();
                    nmc.SetPlayerID(playerID);

                    Debug.Log("Setting Player ID to " + playerID.ToString());

                    var packet = PacketHelperClient.MakeWelcomePacket(WelcomeMessage.RequestSpawn);
                    nmc.SendPacket(packet);
                }
                break;

            // Set Player Network ID and Request Start
            case WelcomeMessage.Spawn:
                {
                    nmc.state = NetworkState.Welcomed;

                    int playerID = reader.ReadInt32();
                    int networkID = reader.ReadInt32();

                    if (playerID == nmc.playerID)
                        nmc.SetPlayerNetworkID(networkID);

                    Debug.Log("Client Player ID " + playerID + " Requesting Client Proxy");
                    var packet = PacketHelperClient.MakeWelcomePacket(WelcomeMessage.RequestBeginPlaying);
                    nmc.SendPacket(packet);
                }
                break;

            // Ready to Play!
            case WelcomeMessage.BeginPlaying:
                {
                    nmc.state = NetworkState.Playing;
                }
                break;
        }

        Debug.Log("CLIENT RECEIVES WELCOME. MESSAGE: " + message.ToString());
    }
}
