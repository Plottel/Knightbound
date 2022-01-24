using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Deft;

public class PlayerControllerClient
{
    public float cameraYSpeed = 1f;

    public void OnUpdate()
    {
        // Console Message
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var consolePacket = PacketHelperClient.MakeConsoleMessagePacket("My First Message");
            NetworkManagerClient.Get.SendPacket(consolePacket);
        }

        // Rotate Camera Left
        if (Input.GetKey(KeyCode.Q))
            CameraManager.Get.RotateY(-cameraYSpeed);

        // Rotate Camera Right
        if (Input.GetKey(KeyCode.E))
            CameraManager.Get.RotateY(cameraYSpeed);
    }

    
}