using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Deft;
using Deft.Networking;

public class InputProcessorServer : Manager<InputProcessorServer>
{
    private Dictionary<int, InputState> playerIDToInputState;

    public override void OnAwake()
    {
        base.OnAwake();
        playerIDToInputState = new Dictionary<int, InputState>();

        PlayerManagerServer.Get.eventPlayerJoined += OnPlayerJoined;
    }

    void OnDestroy() => PlayerManagerServer.Get.eventPlayerJoined -= OnPlayerJoined;

    // TODO: Factor out into Data Singleton storing all Inputs of all Clients
    public InputState[] InputStates
    {
        get => playerIDToInputState.Values.ToArray();
    }

    private void OnPlayerJoined(PlayerInfo playerInfo)
    {
        playerIDToInputState[playerInfo.playerID] = null;
    }

    public void AddInputState(int playerID, InputState inputState)
        => playerIDToInputState[playerID] = inputState;

    public override void OnUpdate()
    {
        ProcessPlayerInputStates();
    }

    void ProcessPlayerInputStates()
    {
        var rms = ReplicationManagerServer.Get;

        foreach (var playerInputInfo in playerIDToInputState)
        {
            int playerID = playerInputInfo.Key;
            InputState inputState = playerInputInfo.Value;

            if (inputState == null)
                continue;

            int characterID = PlayerManagerServer.Get.GetPlayerInfo(playerID).characterID;
            if (ReplicationManagerServer.Get.TryGetNetworkObject(characterID, out var playerBase))
            {
                Player player = playerBase as Player; // Lol so bad - find a T solution
                ApplyInputState(player, inputState);
            }
        }

        // Once Input States have been processed, clear them to avoid duplication.
        // Later, we will store them in a buffer.
        playerIDToInputState.Clear();
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
