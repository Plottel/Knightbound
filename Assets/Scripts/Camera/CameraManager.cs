using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class CameraManager : Manager<CameraManager>
{
    private CharacterCamera characterCamera;

    public override void OnAwake()
    {
        base.OnAwake();

        characterCamera = new GameObject("CharacterCamera").AddComponent<CharacterCamera>();
        characterCamera.transform.parent = transform;
    }

    public void RotateY(float delta) => characterCamera.RotateY(delta);
}
