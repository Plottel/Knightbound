using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceReader
{
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode cameraLeft = KeyCode.Q;
    public KeyCode cameraRight = KeyCode.E;

    public InputState ReadDeviceState()
    {
        return new InputState
        {
            playerID = -1, // TODO: Input State shouldn't need Player ID.
            movement = ReadMovement(),
            cameraLeft = Input.GetKey(cameraLeft),
            cameraRight = Input.GetKey(cameraRight)
        };
    }

    Vector2 ReadMovement()
    {
        float x = 0;
        float z = 0;

        if (Input.GetKey(up)) z = 1f;
        if (Input.GetKey(down)) z = -1f;
        if (Input.GetKey(left)) x = -1f;
        if (Input.GetKey(right)) x = 1f;

        return new Vector2(x, z).normalized;
    }
}
