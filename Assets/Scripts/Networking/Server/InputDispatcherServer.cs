using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;
using Deft.Networking;

// Dispatches Input received from all Clients to all Clients
// We no longer send Player transform state via default ReplicationManager.
// We use a specialized system of serializing inputs and applying them.
public class InputDispatcherServer : Manager<InputDispatcherServer>
{
    public override void OnUpdate()
    {
        DispatchPlayerInputStates();
    }

    void DispatchPlayerInputStates()
    {
        var inputStates = InputProcessorServer.Get.InputStates;

        foreach (InputState input in inputStates)
        {
            var packet = PacketHelper.MakeInputPacket(input.playerID, input);
            NetworkManagerServer.Get.BroadcastPacket(packet);
        }
    }
}
