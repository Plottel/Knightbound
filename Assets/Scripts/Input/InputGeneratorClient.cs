using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Deft;

public class InputGeneratorClient : Manager<InputGeneratorClient>
{
    public override void OnUpdate()
    {
        var inputBuffer = InputBufferClient.Get;

        inputBuffer.ClearLocalBuffer();
        inputBuffer.AddLocalInputState(GenerateInputState());
    }


    InputState GenerateInputState()
    {
        return new InputState
        {
            playerID = NetworkManagerClient.Get.playerID, // TODO: Data Singleton ConnectionInfo something?
            up = Input.GetKey(KeyCode.W),
            down = Input.GetKey(KeyCode.S),
            left = Input.GetKey(KeyCode.A),
            right = Input.GetKey(KeyCode.D),
            cameraLeft = Input.GetKey(KeyCode.Q),
            cameraRight = Input.GetKey(KeyCode.E)
        };
    }
}