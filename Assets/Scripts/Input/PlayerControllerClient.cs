using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Deft;

public class PlayerControllerClient
{
    public Tree player;
    public int playerID;

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var consolePacket = PacketHelperClient.MakeConsoleMessagePacket("My First Message");
            NetworkManagerClient.Get.SendPacket(consolePacket);
        }

        var inputState = new InputState
        {
            move = Input.GetKeyDown(KeyCode.W)
        };

        var inputPacket = PacketHelperClient.MakeInputPacket(inputState, playerID);
        NetworkManagerClient.Get.SendPacket(inputPacket);
    }
}
