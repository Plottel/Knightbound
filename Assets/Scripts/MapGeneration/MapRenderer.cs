using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    public Color errorColor;

    [Header("Height Map")]
    public float[] heightThresholds;
    public Color[] heightColors;

    public Texture2D GenerateMapTexture(MapData data)
    {
        var result = new Texture2D(data.width, data.depth);
        var pixels = new Color[data.width * data.depth];

        // Generate Height Map Texture
        for (int x = 0; x < data.width; ++x)
        {
            for (int z = 0; z < data.depth; ++z)
                pixels[z * data.depth + x] = HeightToColor(data.heightMap[x, z]);
        }

        // Finalize Texture
        result.SetPixels(pixels);
        result.Apply();

        return result;
    }

    Color HeightToColor(float height)
    {
        for (int i = 0; i < heightColors.Length; ++i)
        {
            if (height <= heightThresholds[i])
                return heightColors[i];
        }

        return errorColor;
    }
}
