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
        var nms = NetworkManagerServer.Get;

        if (nms.GetPlayerInfo(peerID, out PlayerInfo playerInfo))
        {
            var inputState = new InputState();
            inputState.Deserialize(reader);

            InputManagerServer.Get.AddInputState(playerInfo, inputState);
        }
    }
}
