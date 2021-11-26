using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConsoleMessagePacketHandlerServer : PacketHandlerServer
{
    public override void HandlePacket(string originIP, BinaryReader reader)
    {
        var p = new ConsoleMessagePacket();
        p.DeserializeHeader(reader);
        p.DeserializeBody(reader);

        // Forward packet to all Clients. When they receive it, they'll print to console.
        server.BroadcastPacket(PacketType.ConsoleMessage, p);
    }
}
