using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputPacketHandlerServer : PacketHandlerServer
{
    public override void HandlePacket(uint peerID, BinaryReader reader)
    {
        int playerID = reader.ReadInt32();
        var inputState = new InputState();
        inputState.Deserialize(reader);

        ClientProxy client = NetworkManagerServer.Get.GetClientProxy(playerID);
        client.AddInputState(inputState);
    }
}
