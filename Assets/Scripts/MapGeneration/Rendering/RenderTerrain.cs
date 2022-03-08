using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTerrain : MapRenderPass
{
    public Color ErrorColor = new Color(1, 0, 1);

    public Color[] TerrainColors;

    public override void Execute(MapData data, Color[] pixels)
    {
        for (int x = 0; x < data.width; ++x)
        {
            for (int z = 0; z < data.depth; ++z)
                pixels[z * data.depth + x] = TerrainToColor(data.terrainMap[x, z]);
        }
    }

    Color TerrainToColor(int terrain)
    {
        if (terrain < 0 || terrain >= TerrainColors.Length)
            return ErrorColor;
        return TerrainColors[terrain];
    }
}
