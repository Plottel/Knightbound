using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTerrain : MapFabricationPass
{
    public Color[] terrainColors;

    public override void Execute(MapData data, Color[] pixels)
    {
        for (int x = 0; x < data.width; ++x)
        {
            for (int z = 0; z < data.depth; ++z)
                pixels[z * data.depth + x] = terrainColors[data.terrainMap[x, z]];
        }
    }
}
