using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;
using Deft.Networking;

public class InputManagerServer : Manager<InputManagerServer>
{
    private Dictionary<PlayerInfo, InputState> clientInputStates;

    public override void OnAwake()
    {
        base.OnAwake();
        clientInputStates = new Dictionary<PlayerInfo, InputState>();

        NetworkManagerServer.Get.eventPlayerJoined += OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInfo playerInfo)
    {
        clientInputStates.Add(playerInfo, null);
    }

    void OnDestroy() => NetworkManagerServer.Get.eventPlayerJoined -= OnPlayerJoined;

    public void AddInputState(PlayerInfo playerInfo, InputState inputState)
        => clientInputStates[playerInfo] = inputState;

    public override void OnUpdate()
    {
        ProcessClientInputStates();
    }

    void ProcessClientInputStates()
    {
        var rms = ReplicationManagerServer.Get;

        foreach (var clientInputInfo in clientInputStates)
        {
            PlayerInfo playerInfo = clientInputInfo.Key;
            InputState inputState = clientInputInfo.Value;

            if (inputState == null)
                continue;

            if (rms.TryGetNetworkObject(playerInfo.networkID, out var playerNetworkObject))
            {
                var player = playerNetworkObject as Player; // Lol so bad - find a T solution.
                ApplyInputState(player, inputState);
            }
        }

        // Once Input States have been processed, clear them to avoid duplication.
        // Later, we will store them in a buffer.
        clientInputStates.Clear();
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
