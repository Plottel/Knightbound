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
            movement = GetMovementInput(),
            cameraLeft = Input.GetKey(KeyCode.Q),
            cameraRight = Input.GetKey(KeyCode.E)
        };
    }

    Vector2 GetMovementInput()
    {
        float x = 0;
        float z = 0;

        Transform camera = CameraManager.Get.Camera;

        if (Input.GetKey(KeyCode.W)) z = 1f;
        if (Input.GetKey(KeyCode.S)) z = -1f;
        if (Input.GetKey(KeyCode.A)) x = -1f;
        if (Input.GetKey(KeyCode.D)) x = 1f;

        Vector3 input = new Vector3(x, 0, z);
        Vector3 relativeInput = camera.TransformDirection(input);
        relativeInput.y = 0;
        relativeInput.Normalize();

        return new Vector2(relativeInput.x, relativeInput.z);
    }
}