using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WelcomePacketHandlerServer : PacketHandlerServer
{
    public override void HandlePacket(string originIP, BinaryReader reader)
    {
        var p = new WelcomePacket();
        p.DeserializeHeader(reader);
        p.DeserializeBody(reader);

        if (p.message == "HELLO")
        {
            // Add new Client
            if (!server.HasIP(originIP))
            {
                int playerID = server.RegisterNewPlayer(originIP);

                // Respond with Welcome Packet containing PlayerID
                var welcomePacket = new WelcomePacket();
                welcomePacket.message = "WELCOME";
                welcomePacket.playerID = playerID;

                server.SendPacket(playerID, PacketType.Welcome, welcomePacket);
            }
        }

        Debug.Log("SERVER: Welcome Packet. Message: " + p.message + " ID: " + p.playerID);
    }
}
