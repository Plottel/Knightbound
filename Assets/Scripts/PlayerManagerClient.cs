using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class PlayerManagerClient : Manager<PlayerManagerClient>
{
    private Dictionary<int, int> playerIDToCharacterID;

    public override void OnAwake()
    {
        base.OnAwake();
        playerIDToCharacterID = new Dictionary<int, int>();
    }

    public void SetPlayerInfo(int playerID, int characterID)
    {
        playerIDToCharacterID.Add(playerID, characterID);
    }

    public int GetCharacterID(int playerID)
        => playerIDToCharacterID[playerID];
}
