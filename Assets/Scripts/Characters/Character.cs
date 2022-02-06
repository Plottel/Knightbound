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
    public CharacterData data;

    // Components
    [HideInInspector] public CharacterMesh mesh;
    [HideInInspector] public CharacterAnimator animator;

    private void FetchReferences()
    {
        mesh = GetComponent<CharacterMesh>();
        animator = GetComponent<CharacterAnimator>();
    }

    void Awake()
    {
        FetchReferences();
    }    

    void OnValidate() 
        => FetchReferences();

    #region GIZMOS
    void OnDrawGizmos()
    {
        if (data != null && data.armatureData != null)
        {
            string[] boneNames = data.armatureData.boneNames;

            foreach (string boneName in boneNames)
            {
                if (mesh.transform.TryFindTransform(boneName, out Transform bone))
                {
                    Gizmos.DrawSphere(bone.position, 0.1f);
                }
            }
        }
    }
    #endregion
}
