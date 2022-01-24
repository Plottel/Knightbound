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
        int playerID = reader.ReadInt32();

        var inputState = new InputState();
        inputState.Deserialize(reader);

        InputProcessorServer.Get.AddInputState(playerID, inputState);
    }
}
