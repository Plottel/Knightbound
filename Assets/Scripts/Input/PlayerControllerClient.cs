using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Deft;

public class PlayerControllerClient
{
    public float cameraYSpeed = 1f;

    private const float kInputPacketSendInterval = 0.033f;
    private float timeSinceLastInputPacket;

    private InputState inputState;

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

        // Input Packets
        timeSinceLastInputPacket += Time.deltaTime;
        if (timeSinceLastInputPacket > kInputPacketSendInterval)
        {
            timeSinceLastInputPacket = 0f;
            SendInputPacket();
        }
    }

    private void SendInputPacket()
    {
        var nmc = NetworkManagerClient.Get;

        var inputState = new InputState
        {
            up = Input.GetKey(KeyCode.W),
            down = Input.GetKey(KeyCode.S),
            left = Input.GetKey(KeyCode.A),
            right = Input.GetKey(KeyCode.D)
        };

        var inputPacket = PacketHelperClient.MakeInputPacket(nmc.playerID, inputState);
        nmc.SendPacket(inputPacket);
    }
}