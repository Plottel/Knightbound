using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTerrain : MapRenderPass
{
    public Color ErrorColor = new Color(1, 0, 1);

    [SerializeReference] 
    public Dictionary<BlockType, Color> TerrainColors = new Dictionary<BlockType, Color>();

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
        if (TerrainColors.TryGetValue((BlockType)terrain, out Color color))
            return color;
        return ErrorColor;
    }
}
