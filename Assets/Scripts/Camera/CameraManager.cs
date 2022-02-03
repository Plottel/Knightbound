using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;
using Deft.Networking;

// TODO: Should be "Nothing-to-do-with-Networking"
public class CameraManager : Manager<CameraManager>
{
    private CharacterCamera characterCamera;

    public override void OnAwake()
    {
        base.OnAwake();

        characterCamera = new GameObject("CharacterCamera").AddComponent<CharacterCamera>();
        characterCamera.transform.parent = transform;
        characterCamera.camera = FindObjectOfType<Camera>().transform; // TODO: Factor out.
        characterCamera.TargetOffset = new Vector3(0, 20, -10);
        characterCamera.SetDefaultRotation(60, 0, 0);
    }

    public override void OnStart()
    {
        GameStateManager.Get.eventEnterGameState += OnEnterGameState;
    }

    private void OnDestroy()
        => GameStateManager.Get.eventEnterGameState -= OnEnterGameState;

    private void OnEnterGameState(GameState state)
    {
        if (state != GameState.InGame)
            return;

        int characterID = PlayerManagerClient.Get.GetCharacterID(NetworkManagerClient.Get.playerID);

        if (ReplicationManagerClient.Get.TryGetNetworkObject(characterID, out var player))
            characterCamera.SetTarget(player.transform);
        else
            Debug.Log("CameraManager could not find Network ID " + characterID);
    }

    public void RotateY(float delta) => characterCamera.RotateAroundTarget(delta);
}
