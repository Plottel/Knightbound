using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class CharacterMovementSystem : Manager<CharacterMovementSystem>
{
    public override void OnUpdate()
    {
        var pms = PlayerManagerServer.Get;
        var rms = ReplicationManagerServer.Get;

        PlayerInfo[] playerList = pms.GetPlayerList();

        foreach (var playerInfo in playerList)
        {
            if (rms.TryGetNetworkObject(playerInfo.characterID, out Player player))
                MovePlayer(player);
        }
    }

    void MovePlayer(Player player)
    {
        Vector3 moveDelta = player.direction * player.speed;
        Vector3 flatMoveDelta = new Vector3(moveDelta.x, 0, moveDelta.z);

        player.transform.position += moveDelta * Time.deltaTime;

        if (moveDelta != Vector3.zero)
            player.transform.LookAt(player.transform.position + flatMoveDelta);
    }
}
