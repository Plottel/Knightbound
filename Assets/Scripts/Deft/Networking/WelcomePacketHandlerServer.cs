using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class WelcomePacketHandlerServer : PacketHandlerServer
{

    public override void HandlePacket(uint peerID, BinaryReader reader)
    {
        NetworkManagerServer nms = NetworkManagerServer.Get;
        var state = (WelcomeState)reader.ReadInt32();

        int playerID = 0;

        switch (state)
        {
            // Generate a Client Proxy and send the Player ID back to the client.
            case WelcomeState.PlayerID:
                // Don't make duplicate client proxies
                if (!nms.HasPeerID(peerID))
                {
                    // Don't make a client proxy for the local client
                    if (peerID > 0)
                    {
                        // Need to decouple ClientProxy from Player ID
                        ClientProxy client = nms.RegisterNewPlayer(peerID);
                    }



                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
                        {
                            writer.Write((int)PacketType.Welcome);
                            writer.Write((int)WelcomeState.PlayerID);
                            //writer.Write(client.playerID);
                        }

                        //nms.SendPacket(client.playerID, stream);
                    }
                }

                break;

            // Generate a Player Object and send the Network ID back to the client.
            case WelcomeState.PlayerObjectID:
                // How to prevent creating duplicate players.. is this even necessary?
                if (nms.GetPlayerID(peerID, out playerID))
                {
                    int playerObjNetworkID;
                    nms.CreateNetworkObject<Tree>(NetworkObjectType.Tree, out playerObjNetworkID);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
                        {
                            writer.Write((int)PacketType.Welcome);
                            writer.Write((int)WelcomeState.PlayerObjectID);
                            writer.Write(playerObjNetworkID);
                        }

                        nms.SendPacket(playerID, stream);
                    }
                }

                break;

        }

        Debug.Log("SERVER: Welcome Packet. Message: " + state.ToString());
    }
}
