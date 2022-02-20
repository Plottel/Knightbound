using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// NOTE: More Advanced Solution Required...
public struct LineSegment
{
    public Vector2 start;
    public Vector2 end;
}

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
        Debug.Log("~~~~~ Serialize - W: " + width + " D: " + depth);

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

        Debug.Log("~~~~~ Deserialize - W: " + width + " D: " + depth);
    }
}
