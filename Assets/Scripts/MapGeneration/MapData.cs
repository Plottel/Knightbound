using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapData
{
    public int seed;
    public int width;
    public int depth;
    public Vector2Int spawn;
    public float[,] heightMap;
    public int[,] terrainMap;
    public LineSegment[] roads;

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(width);
        writer.Write(depth);

        for (int x = 0; x < width; ++x)
        {
            for (int z = 0; z < depth; ++z)
                writer.Write(terrainMap[x, z]);
        }
    }

    public void Deserialize(BinaryReader reader)
    {
        width = reader.ReadInt32();
        depth = reader.ReadInt32();

        terrainMap = new int[width, depth];

        for (int x = 0; x < width; ++x)
        {
            for (int z = 0; z < depth; ++z)
                terrainMap[x, z] = reader.ReadInt32();
        }
    }
}
