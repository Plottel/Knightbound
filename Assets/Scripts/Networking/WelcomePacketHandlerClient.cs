using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WelcomePacketHandlerClient : PacketHandlerClient
{
    public override void HandlePacket(string originIP, BinaryReader reader)
    {
        var p = new WelcomePacket();
        p.DeserializeHeader(reader);
        p.DeserializeBody(reader);

        if (p.message == "WELCOME")
        {
            client.State = GameClientState.Connected;
            client.playerID = p.playerID;
        }
    }
}
