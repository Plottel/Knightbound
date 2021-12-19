using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerClient
{
    public Tree player;

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NetworkManagerClient.Get.SendConsoleMessagePacket("My First Message");
        }

        var inputState = new InputState
        {
            move = Input.GetKeyDown(KeyCode.W)
        };

        NetworkManagerClient.Get.SendInputPacket(inputState);
    }
}
