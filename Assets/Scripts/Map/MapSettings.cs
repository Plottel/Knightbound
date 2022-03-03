using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct MapSettings
{
    public string name;
    public int seed;
    public int width;
    public int depth;

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(name);
        writer.Write(seed);
        writer.Write(width);
        writer.Write(depth);
    }

    public void Deserialize(BinaryReader reader)
    {
        name = reader.ReadString();
        seed = reader.ReadInt32();
        width = reader.ReadInt32();
        depth = reader.ReadInt32();
    }
}
