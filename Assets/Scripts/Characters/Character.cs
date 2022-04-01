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

    // Movement Fields (TODO: Factor out into DeftCharacterController or equivalent?)
    [HideInInspector] public Vector3 direction;
    public float speed = 3f;

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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.LogWarning("CHARACTER - ControllerColliderHit " + hit.gameObject.name);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.LogWarning("CHARACTER - CollisionEnter" + collision.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("CHARACTER - TriggerEnter" + other.gameObject.name);
    }

    public override void Serialize(BinaryWriter writer)
    {
        writer.Write(transform.position.x);
        writer.Write(transform.position.y);
        writer.Write(transform.position.z);

        writer.Write(transform.rotation.x);
        writer.Write(transform.rotation.y);
        writer.Write(transform.rotation.z);
        writer.Write(transform.rotation.w);
    }

    public override void Deserialize(BinaryReader reader)
    {
        float xPos = reader.ReadSingle();
        float yPos = reader.ReadSingle();
        float zPos = reader.ReadSingle();

        transform.position = new Vector3(xPos, yPos, zPos);

        float xRot = reader.ReadSingle();
        float yRot = reader.ReadSingle();
        float zRot = reader.ReadSingle();
        float wRot = reader.ReadSingle();

        transform.rotation = new Quaternion(xRot, yRot, zRot, wRot);
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

        Gizmos.DrawLine(transform.position, transform.position + direction);
    }
    #endregion
}
