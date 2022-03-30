using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;
using Deft.Networking;

// Processes Received Input from the server
public class ReceivedInputProcessorClient : Manager<ReceivedInputProcessorClient>
{
    public override void OnUpdate()
    {
    }

    public override void OnLateUpdate()
    {
        InputBufferClient.Get.ClearReceivedBuffers();
    }
}