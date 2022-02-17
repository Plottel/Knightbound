using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrainMap : MapGenerationPass
{
    public float[] TerrainHeightThresholds = new float[0];

    public override void Execute(MapData data)
    {
        data.terrainMap = new int[data.width, data.depth];

        for (int x = 0; x < data.width; ++x)
        {
            for (int z = 0; z < data.depth; ++z)
                data.terrainMap[x, z] = HeightToTerrainIndex(data.heightMap[x, z]);
        }
    }

    int HeightToTerrainIndex(float height)
    {
        for (int i = 0; i < TerrainHeightThresholds.Length; ++i)
        {
            if (height <= TerrainHeightThresholds[i])
                return i;
        }

        return TerrainHeightThresholds.Length - 1; // If above max threshold, set to max terrain.
    }
}
