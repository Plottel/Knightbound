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
    private CharacterMesh characterMesh;
    private CharacterAnimator characterAnimator;

    void Awake()
    {
        FetchReferences();
        SetupAnimator();
    }

    void Start()
    {
        characterAnimator.Play(0);
    }

    void OnValidate() 
        => FetchReferences();

    void OnDrawGizmos()
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

    private void FetchReferences()
    {
        characterMesh = GetComponent<CharacterMesh>();
        characterAnimator = GetComponent<CharacterAnimator>();
    }

    public void AttachMeshes()
    {
        ArmatureData armatureData = characterData.armatureData;
        Transform rootBone = Instantiate(characterData.armature);

        characterMesh.SetBoneData(rootBone, armatureData.boneNames);
        characterMesh.AttachMeshes(characterData.defaultMeshPieces);
    }

    public void DestroyMeshes()
    {
        characterMesh.DestroyMeshes();
    }

    public void SetupAnimator()
    {
        AnimatorData animatorData = characterData.animatorData;

        characterAnimator.SetAnimatorController(animatorData.animatorController);
        // Does it matter that Animator is referencing array from Animation asset?
        characterAnimator.SetAnimations(characterData.animatorData.animations);
    }
}
