using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Deft;
using Deft.Networking;

public class ReceivedInputProcessorServer : Manager<ReceivedInputProcessorServer>
{
    public override void OnUpdate()
    {
        ProcessClientInputStates();
    }

    void ProcessClientInputStates()
    {
        var rms = ReplicationManagerServer.Get;
        var buffers = InputBufferServer.Get.GetAllBuffers();

        // For each Player's Buffer
        foreach (List<InputState> inputBuffer in buffers)
        {
            if (inputBuffer.Count == 0)
                continue;

            int characterID = PlayerManagerServer.Get.GetPlayerInfo(inputBuffer[0].playerID).characterID;

            // Try to fetch the Player
            if (rms.TryGetNetworkObject(characterID, out var playerBase))
            {
                Player player = playerBase as Player; // TODO: Lol so bad - find a T solution.

                // Process All the Input Packets for this Player
                foreach (InputState state in inputBuffer)
                    ApplyInputState(player, state);
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
