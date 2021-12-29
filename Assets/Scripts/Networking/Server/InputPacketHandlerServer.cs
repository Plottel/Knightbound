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

        if (nms.GetClientInfo(peerID, out ClientInfo clientInfo))
        {
            var inputState = new InputState();
            inputState.Deserialize(reader);

            var clientProxy = nms.GetClientProxy(clientInfo.playerID);
            clientProxy.AddInputState(inputState);
        }
    }
}
