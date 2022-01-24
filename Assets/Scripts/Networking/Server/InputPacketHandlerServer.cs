using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Deft;
using Deft.Networking;

public class InputPacketHandlerServer : PacketHandlerServer
{
    public override void HandlePacket(uint peerID, BinaryReader reader)
    {
        var input = new InputState();
        input.Deserialize(reader);

        InputBufferServer.Get.AddInputState(input);
    }
}
