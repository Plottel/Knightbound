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

    // // TODO: "Advance The Simulation"
    void ProcessClientInputStates()
    {
        var rms = ReplicationManagerServer.Get;
        var inputBuffer = InputBufferServer.Get.GetBuffer();

        // TODO: AWFUL Data access pattern!!!
        foreach (InputState input in inputBuffer)
        {
            int characterID = PlayerManagerServer.Get.GetPlayerInfo(input.playerID).characterID;
            if (ReplicationManagerServer.Get.TryGetNetworkObject(characterID, out var playerBase))
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
        
        // TODO: To Advance the simulation.. ACTUALLY move the player.
        // We now need simulation time as player's movement CANNOT be based
        // upon the number of Input Packets received.
    }
}
