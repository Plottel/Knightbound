using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class PlayerManagerClient : Manager<PlayerManagerClient>
{
    public delegate void PlayerJoinedHandler(int playerID, int characterID);
    public event PlayerJoinedHandler eventPlayerJoined;

    private Dictionary<int, int> playerIDToCharacterID;

    public override void OnAwake()
    {
        base.OnAwake();
        playerIDToCharacterID = new Dictionary<int, int>();
    }

    public void SetPlayerInfo(int playerID, int characterID)
    {
        playerIDToCharacterID.Add(playerID, characterID);
        eventPlayerJoined?.Invoke(playerID, characterID);
    }

    public int GetCharacterID(int playerID)
    {
        if (playerIDToCharacterID.ContainsKey(playerID))
            return playerIDToCharacterID[playerID];
        else
        {
            Debug.LogWarning("Cannot find Character ID for Player ID " + playerID.ToString());
            return -1;
        }
    }
}
