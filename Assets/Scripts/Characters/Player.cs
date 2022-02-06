using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft.Networking;
using System.IO;

public class Player : NetworkObject
{
    public override int GetClassID() => (int)NetworkObjectType.Player;

    public Vector3 direction;
    private float speed = 5f;

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
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
}
