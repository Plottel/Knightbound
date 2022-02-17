using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    [SerializeReference]
    public List<MapRenderPass> passes = new List<MapRenderPass>();

    public Texture2D GenerateMapTexture(MapData data)
    {
        var result = new Texture2D(data.width, data.depth);
        var pixels = new Color[data.width * data.depth];

        foreach (MapRenderPass pass in passes)
            pass.Execute(data, pixels);

        // Finalize Texture
        result.SetPixels(pixels);
        result.Apply();

        return result;
    }
}
