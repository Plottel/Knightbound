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
        var state = (WelcomeState)reader.ReadInt32();

        switch (state)
        {
            case WelcomeState.PlayerID:
                int playerID = reader.ReadInt32();
                nmc.SetPlayerID(playerID);

                using (MemoryStream stream = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
                    {
                        writer.Write((int)PacketType.Welcome);
                        writer.Write((int)WelcomeState.PlayerObjectID);
                    }

                    nmc.SendPacket(stream);
                }

                break;

            case WelcomeState.PlayerObjectID:
                int playerObjNetworkID = reader.ReadInt32();
                nmc.SetPlayerObjNetworkID(playerObjNetworkID);

                // Now ready to request Game Data..

                break;

        }
    }
}
