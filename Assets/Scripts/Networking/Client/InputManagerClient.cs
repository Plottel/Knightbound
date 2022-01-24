using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class InputManagerClient : Manager<InputManagerClient>
{
    public InputState GenerateInputState()
    {
        return new InputState
        {
            playerID = NetworkManagerClient.Get.playerID, // TODO: Data Singleton ConnectionInfo something?
            up = Input.GetKey(KeyCode.W),
            down = Input.GetKey(KeyCode.S),
            left = Input.GetKey(KeyCode.A),
            right = Input.GetKey(KeyCode.D)
        };
    }
}
