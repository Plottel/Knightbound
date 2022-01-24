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

    private PlayerControllerClient controller;

    public override void OnAwake()
    {
        base.OnAwake();
        controller = new PlayerControllerClient();
    }

    public override void OnUpdate()
    {
        // TODO: Initialize these Managers in response to NetworkState becoming Playing?
        if (NetworkManagerClient.Get.state == NetworkState.Playing)
        {
            timeSinceLastInputPacket += Time.deltaTime;
            if (timeSinceLastInputPacket > kInputPacketSendInterval)
            {
                timeSinceLastInputPacket = 0f;
                DispatchPlayerInputState();
            }
        }
    }

    void DispatchPlayerInputState()
    {
        var nmc = NetworkManagerClient.Get; // TODO: Input Data Singleton...
        var inputState = InputManagerClient.Get.GenerateInputState();
        var inputPacket = PacketHelper.MakeInputPacket(nmc.playerID, inputState);

        nmc.SendPacket(inputPacket);
    }

    void SendInputPacket()
    {
        var nmc = NetworkManagerClient.Get;

        var inputState = new InputState
        {
            playerID = nmc.playerID,
            up = Input.GetKey(KeyCode.W),
            down = Input.GetKey(KeyCode.S),
            left = Input.GetKey(KeyCode.A),
            right = Input.GetKey(KeyCode.D)
        };

        var inputPacket = PacketHelper.MakeInputPacket(nmc.playerID, inputState);
        nmc.SendPacket(inputPacket);
    }
}
