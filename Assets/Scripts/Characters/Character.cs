using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

[RequireComponent(typeof(CharacterMesh))]
[RequireComponent(typeof(CharacterAnimator))]
public class Character : MonoBehaviour
{
    // Data
    public CharacterData characterData;

    // Components
    [HideInInspector] public CharacterMesh characterMesh;
    [HideInInspector] public CharacterAnimator characterAnimator;

    private void FetchReferences()
    {
        characterMesh = GetComponent<CharacterMesh>();
        characterAnimator = GetComponent<CharacterAnimator>();
    }

    void Awake()
    {
        FetchReferences();
    }

    void Start()
    {
        characterAnimator.Play(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            characterAnimator.Play(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            characterAnimator.Play(1);
    }

    void OnValidate() 
        => FetchReferences();

    void OnDrawGizmos()
    {
        if (characterData != null && characterData.armatureData != null)
        {
            string[] boneNames = characterData.armatureData.boneNames;

            foreach (string boneName in boneNames)
            {
                if (characterMesh.transform.TryFindTransform(boneName, out Transform bone))
                {
                    Gizmos.DrawSphere(bone.position, 0.1f);
                }
            }
        }
    }
}
