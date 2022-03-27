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
        //ProcessClientInputStates();
    }

    public override void OnLateUpdate()
    {
        InputBufferClient.Get.ClearReceivedBuffers();
    }

    void ProcessClientInputStates()
    {
        var rmc = ReplicationManagerClient.Get;
        var buffers = InputBufferClient.Get.GetAllReceivedBuffers();

        // For each Player's Buffer
        foreach (List<InputState> inputBuffer in buffers)
        {
            if (inputBuffer.Count == 0)
                continue;

            int characterID = PlayerManagerClient.Get.GetCharacterID(inputBuffer[0].playerID);

            // Try to fetch the Player
            if (rmc.TryGetNetworkObject(characterID, out var playerBase))
            {
                Player player = playerBase as Player; // TODO: Lol so bad - find a T solution.

                // Process All the Input Packets for this Player
                foreach (InputState state in inputBuffer)
                    ApplyInputState(player, state);

                // Move this Player after all Input Packets processed
                // TODO: Separate system. This is just for applying input states.
                //MovePlayer(player);
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

    void MovePlayer(Player player)
    {
        Vector3 moveDelta = player.direction * player.speed;
        player.transform.position += moveDelta;
        //player.controller.SimpleMove(moveDelta);
    }
}