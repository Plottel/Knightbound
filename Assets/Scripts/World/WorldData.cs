using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldData
{
    public int[,] blocks;

    public int Width => blocks.GetLength(0);
    public int Depth => blocks.GetLength(1);    

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(Width);
        writer.Write(Depth);

        for (int x = 0; x < Width; ++x)
        {
            for (int z = 0; z < Depth; ++z)
                writer.Write(blocks[x, z]);
        }
    }

    public void Deserialize(BinaryReader reader)
    {
        int width = reader.ReadInt32();
        int depth = reader.ReadInt32();

        blocks = new int[width, depth];

        for (int x = 0; x < Width; ++x)
        {
            for (int z = 0; z < Depth; ++z)
                blocks[x, z] = reader.ReadInt32();
        }
    }
}
