using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapRenderer
{
    public static Texture2D GenerateMapTexture(MapData data, MapRenderSettings settings)
    {
        if (settings == null)
            return new Texture2D(0, 0);

        var result = new Texture2D(data.width, data.depth);
        var pixels = new Color[data.width * data.depth];

        foreach (MapRenderPass pass in settings.passes)
            pass.Execute(data, pixels);

        // Finalize Texture
        result.SetPixels(pixels);
        result.Apply();

        return result;
    }
}
