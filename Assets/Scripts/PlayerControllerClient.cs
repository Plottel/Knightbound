using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;
using Deft.Networking;

public class PlayerControllerClient
{
    public Tree player;

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DeftNetworkManagerClient.Get.SendConsoleMessagePacket("My First Message");
        }

        var inputState = new InputState
        {
            move = Input.GetKeyDown(KeyCode.W)
        };

        DeftNetworkManagerClient.Get.SendInputPacket(inputState);
    }
}
