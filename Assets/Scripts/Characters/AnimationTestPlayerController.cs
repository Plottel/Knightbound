using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTestPlayerController : MonoBehaviour
{
    public Character character;

    private void Start()
    {
        character.animator.Play(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            character.animator.Play(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            character.animator.Play(1);
    }
}
