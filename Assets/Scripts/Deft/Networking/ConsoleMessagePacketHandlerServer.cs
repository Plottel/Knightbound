using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConsoleMessagePacketHandlerServer : PacketHandlerServer
{
    public override void HandlePacket(uint peerID, BinaryReader reader)
    {
        string message = reader.ReadString();

        // Forward packet to all Clients.
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write((int)PacketType.ConsoleMessage);
                writer.Write(message);
            }

            server.TrueBroadcastPacket(stream);
        }
    }
}
