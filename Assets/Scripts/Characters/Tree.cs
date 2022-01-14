using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Deft.Networking;

public class Tree : NetworkObject
{
    public override int GetClassID() => (int)NetworkObjectType.Tree;

    public string treeName;

    public override void Serialize(BinaryWriter writer) 
    {
        writer.Write(treeName);
        writer.Write(transform.position.x);
        writer.Write(transform.position.y);
        writer.Write(transform.position.z);
    }

    public override void Deserialize(BinaryReader reader)
    {
        treeName = reader.ReadString();

        float x = reader.ReadSingle();
        float y = reader.ReadSingle();
        float z = reader.ReadSingle();

        transform.position = new Vector3(x, y, z);
    }
}
