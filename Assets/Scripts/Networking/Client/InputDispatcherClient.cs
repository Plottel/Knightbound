using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;
using Deft.Networking;

// Dispatches input from the Local Client to the server
public class InputDispatcherClient : Manager<InputDispatcherClient>
{
    private const float kInputPacketSendInterval = 0.033f;
    private float timeSinceLastInputPacket;

    public override void OnUpdate()
    {
        // TODO: Initialize these Managers in response to NetworkState becoming Playing?
        // TODO: Check if this check is even necessary!
        if (NetworkManagerClient.Get.state == NetworkState.Playing)
        {
            timeSinceLastInputPacket += Time.deltaTime;
            if (timeSinceLastInputPacket > kInputPacketSendInterval)
            {
                timeSinceLastInputPacket = 0f;
                DispatchInput();
            }
        }
    }

    void DispatchInput()
    {
        var nmc = NetworkManagerClient.Get; // TODO: Input Data Singleton...

        foreach (var input in InputBufferClient.Get.GetLocalBuffer())
        {
            var inputPacket = PacketHelper.MakeInputPacket(input);
            nmc.SendPacket(inputPacket);
        }
    }
}
