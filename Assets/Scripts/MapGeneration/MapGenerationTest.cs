using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapGenerator), typeof(MapRenderer), typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class MapGenerationTest : MonoBehaviour
{
    public MapGenerator mapGenerator;
    public MapRenderer mapRenderer;

    private MeshRenderer unityRenderer;

    void Awake()
    {
        FetchComponentReferences();
    }

    private void OnValidate()
    {
        FetchComponentReferences();
    }

    [ExecuteInEditMode]
    private void Update()
    {
        UpdateMap();
    }

    public void UpdateMap()
    {
        MapData mapData = mapGenerator.GenerateMapData();
        Texture2D mapTexture = mapRenderer.GenerateMapTexture(mapData);

        unityRenderer.sharedMaterial.mainTexture = mapTexture;
        transform.localScale = new Vector3(mapData.width, 0, mapData.depth);
    }

    public void FetchComponentReferences()
    {
        if (mapGenerator == null) mapGenerator = GetComponent<MapGenerator>();
        if (mapRenderer == null) mapRenderer = GetComponent<MapRenderer>();
        if (unityRenderer == null) unityRenderer = GetComponent<MeshRenderer>();
    }
}
