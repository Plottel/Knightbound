using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class LocalInputProcessor : Manager<LocalInputProcessor>
{
    public override void OnUpdate()
    {
        // Fetch Input Data
        InputState input = InputBufferClient.Get.GetLatestLocalInput();
        float cameraYSpeed = UserInputSettings.Get.cameraYSpeed;

        // Rotate Camera
        if (input.cameraLeft)   CameraManager.Get.RotateY(-cameraYSpeed);
        if (input.cameraRight)  CameraManager.Get.RotateY(cameraYSpeed);
    }
}
