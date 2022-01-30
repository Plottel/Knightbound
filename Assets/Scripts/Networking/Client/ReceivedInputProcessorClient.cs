using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;
using Deft.Networking;

// Processes Received Input from the server
public class ReceivedInputProcessorClient : Manager<ReceivedInputProcessorClient>
{
    public override void OnUpdate()
    {
        ProcessPlayerInputStates();
    }

    public override void OnLateUpdate()
    {
        InputBufferClient.Get.ClearReceivedBuffer();
    }

    void ProcessPlayerInputStates()
    {
        var rms = ReplicationManagerServer.Get;
        var inputBuffer = InputBufferClient.Get.GetReceivedBuffer();

        // TODO: AWFUL Data access pattern..
        foreach (InputState input in inputBuffer)
        {
            int characterID = PlayerManagerClient.Get.GetCharacterID(input.playerID);
            if (ReplicationManagerClient.Get.TryGetNetworkObject(characterID, out var playerBase))
            {
                Player player = playerBase as Player; // Lol so bad - find a T solution
                ApplyInputState(player, input);
            }
        }
    }

    void ApplyInputState(Player player, InputState inputState)
    {
        float x = 0;
        float z = 0;

        if (inputState.up) z = 1f;
        if (inputState.down) z = -1f;
        if (inputState.left) x = -1f;
        if (inputState.right) x = 1f;

        player.direction = new Vector3(x, 0, z);
    }
}