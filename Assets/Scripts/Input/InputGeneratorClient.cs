using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Deft;

public class InputGeneratorClient : Manager<InputGeneratorClient>
{
    public DeviceReader deviceReader;

    NetworkManagerClient nmc;
    CameraManager cm;
    InputBufferClient ibc;

    public override void OnAwake()
    {
        deviceReader = new DeviceReader();
    }

    public override void OnStart()
    {
        nmc = NetworkManagerClient.Get;
        cm = CameraManager.Get;
        ibc = InputBufferClient.Get;
    }

    public override void OnUpdate()
    {
        ibc.ClearLocalBuffer();
        ibc.AddLocalInputState(GenerateInputState());
    }

    InputState GenerateInputState()
    {
        InputState result = deviceReader.ReadDeviceState();

        result.playerID = nmc.playerID; // TODO: Data Singleton ConnectionInfo something?
        result.movement = WorldToCameraSpace(result.movement);

        return result;
    }

    Vector2 WorldToCameraSpace(Vector2 world)
    {
        Vector3 relativeInput = cm.Camera.TransformDirection(new Vector3(world.x, 0, world.y));
        relativeInput.y = 0;
        relativeInput.Normalize();

        return new Vector2(relativeInput.x, relativeInput.z);
    }
}