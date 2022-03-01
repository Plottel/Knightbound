using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrainMap : MapGenerationPass
{
    public float[] TerrainLevels = new float[0];
    public BlockType[] TerrainBlockTypes = new BlockType[0];

    public override void Execute(MapData data)
    {
        data.terrainMap = new int[data.width, data.depth];

        for (int x = 0; x < data.width; ++x)
        {
            for (int z = 0; z < data.depth; ++z)
            {
                int heightLevel = HeightToTerrainLevel(data.heightMap[x, z]);
                data.terrainMap[x, z] = (int)TerrainBlockTypes[heightLevel];
            }
        }
    }

    int HeightToTerrainLevel(float height)
    {
        for (int i = 0; i < TerrainLevels.Length; ++i)
        {
            if (height <= TerrainLevels[i])
                return i;
        }

        return TerrainLevels.Length - 1; // If above max threshold, set to max terrain.
    }
}
