using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;
using Deft.Networking;

public class InputDispatcherServer : Manager<InputDispatcherServer>
{
    public override void OnUpdate()
    {
        DispatchClientInputStates();
    }

    public override void OnLateUpdate()
    {
        InputBufferServer.Get.ClearBuffer();
    }

    void DispatchClientInputStates()
    {
        var inputBuffer = InputBufferServer.Get.GetBuffer();

        foreach (InputState input in inputBuffer)
        {
            var packet = PacketHelper.MakeInputPacket(input);
            NetworkManagerServer.Get.TrueBroadcastPacket(packet);
        }
    }
}
