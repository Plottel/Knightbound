using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class PlayerManagerServer : Manager<PlayerManagerServer>
{
    public delegate void PlayerJoinedHandler(PlayerInfo playerInfo);
    public event PlayerJoinedHandler eventPlayerJoined;

    private Dictionary<int, PlayerInfo> playerIDToPlayerInfo;

    public override void OnAwake()
    {
        base.OnAwake();

        playerIDToPlayerInfo = new Dictionary<int, PlayerInfo>();
    }

    public void RegisterNewPlayer(int playerID)
    {
        var playerInfo = new PlayerInfo
        {
            state = NetworkState.Uninitialized,
            playerID = playerID,
            characterID = -1
        };

        playerIDToPlayerInfo.Add(playerID, playerInfo);
    }

    public PlayerInfo SpawnPlayer(int playerID)
    {
        int characterID;
        ReplicationManagerServer.Get.CreateNetworkObject<Player>((int)NetworkObjectType.Player, out characterID);

        PlayerInfo playerInfo = playerIDToPlayerInfo[playerID];
        playerInfo.state = NetworkState.Welcomed;
        playerInfo.characterID = characterID;

        return playerInfo;
    }

    public void FinalizePlayerWelcome(int playerID)
    {
        PlayerInfo playerInfo = playerIDToPlayerInfo[playerID];
        playerInfo.state = NetworkState.Playing;

        eventPlayerJoined?.Invoke(playerInfo);
    }

    public PlayerInfo GetPlayerInfo(int playerID)
        => playerIDToPlayerInfo[playerID];
}
