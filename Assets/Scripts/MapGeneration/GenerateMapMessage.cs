using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct GenerateMapMessage
{
    public int seed;

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(seed);
    }

    public void Deserialize(BinaryReader reader)
    {
        seed = reader.ReadInt32();
    }
}
