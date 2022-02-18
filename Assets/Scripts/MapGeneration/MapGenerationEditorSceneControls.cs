using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

[ExecuteInEditMode]
public class MapGenerationEditorSceneControls : MonoBehaviour
{
    public int seed;
    public MapGenerationSettings mapGenerationSettings;
    public MapRenderSettings mapRenderSettings;

    private MeshRenderer mapMeshRenderer;

    void OnEnable()
    {
        if (mapMeshRenderer != null)
            return;

        SpawnMapMesh();
    }

    [ExecuteInEditMode]
    private void Update()
    {
        MapData mapData = GenerateMapData();
        Texture2D mapTexture = GenerateMapTexture(mapData);

        ApplyMapTexture(mapTexture, mapMeshRenderer);
    }

    MapData GenerateMapData() 
        => MapGenerator.GenerateMapData(seed, mapGenerationSettings);

    Texture2D GenerateMapTexture(MapData mapData) 
        => MapRenderer.GenerateMapTexture(mapData, mapRenderSettings);

    void ApplyMapTexture(Texture2D texture, MeshRenderer renderer)
    {
        mapMeshRenderer.sharedMaterial.mainTexture = texture;
        mapMeshRenderer.transform.localScale = new Vector3(texture.width, 0, texture.height);
    }

    void SpawnMapMesh()
    {
        var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.parent = transform;
        plane.name = "Map";

        mapMeshRenderer = plane.GetComponent<MeshRenderer>();
        mapMeshRenderer.sharedMaterial = new Material(Shader.Find("Unlit/Texture"));
        mapMeshRenderer.transform.position = Vector3.zero;
    }
}

#endif