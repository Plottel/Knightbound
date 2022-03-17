using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Animancer;
using Deft.Networking;

[RequireComponent(typeof(CharacterMesh))]
[RequireComponent(typeof(CharacterAnimator))]
public class Character : NetworkObject
{
    // Data
    public CharacterData data;

    // Components
    [HideInInspector] public CharacterMesh mesh;
    [HideInInspector] public CharacterAnimator animator;
    [HideInInspector] public CharacterController controller;

    // Movement Fields (TODO: Factor out into DeftCharacterController or equivalent?)
    [HideInInspector] public Vector3 direction;
    private float speed = 5f;

    private void FetchReferences()
    {
        mesh = GetComponent<CharacterMesh>();
        animator = GetComponent<CharacterAnimator>();
        controller = GetComponent<CharacterController>();
    }

    void Awake()
    {
        FetchReferences();
    }    

    void OnValidate() 
        => FetchReferences();

    void Update()
    {
        Vector3 moveDelta = direction * speed;
        controller.SimpleMove(moveDelta);
    }

    public override void Serialize(BinaryWriter writer)
    {
        writer.Write(transform.position.x);
        writer.Write(transform.position.y);
        writer.Write(transform.position.z);
    }

    public override void Deserialize(BinaryReader reader)
    {
        float x = reader.ReadSingle();
        float y = reader.ReadSingle();
        float z = reader.ReadSingle();

        transform.position = new Vector3(x, y, z);
    }

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
