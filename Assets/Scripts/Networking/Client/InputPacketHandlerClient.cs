using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;
using Deft.Networking;
using System.IO;

public class InputPacketHandlerClient : PacketHandlerClient
{
    public override void HandlePacket(string originIP, BinaryReader reader)
    {
        var input = new InputState();
        input.Deserialize(reader);

        InputBufferClient.Get.AddReceivedInputState(input);
    }
}
