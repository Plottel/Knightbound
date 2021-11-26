using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Tree : NetworkObject
{
    public override NetworkObjectType GetClassID() => NetworkObjectType.Tree;

    public string name;

    public override void Serialize(BinaryWriter writer)
    {
        base.Serialize(writer);
        writer.Write(name);
    }

    public override void Deserialize(BinaryReader reader)
    {
        base.Deserialize(reader);
        name = reader.ReadString();
    }
}
