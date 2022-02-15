using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public MapGenerationPass[] passes;

    public int size = 1;
    public int seed = 1;

    void Awake()
    {
        passes = GetComponentsInChildren<MapGenerationPass>();
    }

    void OnValidate()
    {
        passes = GetComponentsInChildren<MapGenerationPass>();
    }

    public MapData GenerateMapData()
    {
        var result = new MapData();

        Random.InitState(seed);

        result.seed = seed;
        result.width = size;
        result.depth = size;

        foreach (MapGenerationPass pass in passes)
            pass.Execute(result);

        return result;
    }
}
