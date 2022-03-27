using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;
using Deft.Networking;

public class InputDispatcherServer : Manager<InputDispatcherServer>
{
    public override void OnUpdate()
    {
        //DispatchClientInputStates();
    }

    public override void OnLateUpdate()
    {
        InputBufferServer.Get.ClearBuffers();
    }

    void DispatchClientInputStates()
    {
        foreach (List<InputState> inputBuffer in InputBufferServer.Get.GetAllBuffers())
        {
            foreach (InputState input in inputBuffer)
            {
                var packet = PacketHelper.MakeInputPacket(input);
                NetworkManagerServer.Get.TrueBroadcastPacket(packet);
            }
        }
    }
}
