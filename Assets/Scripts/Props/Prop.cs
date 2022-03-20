using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Deft.Networking;

public class Prop : NetworkObject
{
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.LogWarning("PROP - ControllerColliderHit" + hit.gameObject.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogWarning("PROP - CollisionEnter" + collision.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("PROP - TriggerEnter" + other.gameObject.name);
    }
}
