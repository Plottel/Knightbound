using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapGenerator), typeof(MapRenderer), typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MapGenerationTest : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public MapRenderer mapRenderer;

    private MeshRenderer unityRenderer;

    private void OnValidate()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        FetchComponentReferences();

        MapData mapData = mapGenerator.GenerateMapData();
        Texture2D mapTexture = mapRenderer.GenerateMapTexture(mapData);

        unityRenderer.sharedMaterial.mainTexture = mapTexture;
        transform.localScale = new Vector3(mapData.width, 0, mapData.depth);
        Debug.Log("Generating!");
    }

    void FetchComponentReferences()
    {
        if (mapGenerator == null) mapGenerator = GetComponent<MapGenerator>();
        if (mapRenderer == null) mapRenderer = GetComponent<MapRenderer>();
        if (unityRenderer == null) unityRenderer = GetComponent<MeshRenderer>();
    }
}
