using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    public MapFabricationPass[] passes;

    public Color errorColor;
    public Color roadColor;

    void Awake()
    {
        passes = GetComponentsInChildren<MapFabricationPass>();
    }

    void OnValidate()
    {
        passes = GetComponentsInChildren<MapFabricationPass>();
    }

    public Texture2D GenerateMapTexture(MapData data)
    {
        var result = new Texture2D(data.width, data.depth);
        var pixels = new Color[data.width * data.depth];

        foreach (MapFabricationPass pass in passes)
            pass.Execute(data, pixels);

        // Finalize Texture
        result.SetPixels(pixels);
        result.Apply();

        return result;
    }
}
