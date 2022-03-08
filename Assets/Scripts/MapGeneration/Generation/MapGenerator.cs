using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerator
{
    public static MapData GenerateMapData(int seed, MapGenerationSettings settings)
    {
        if (settings == null)
            return new MapData();

        var result = new MapData();

        Random.InitState(seed);

        result.seed = seed;
        result.width = settings.size;
        result.depth = settings.size;

        result.heightMap = new float[result.width, result.depth];
        result.terrainMap = new int[result.width, result.depth];
        result.propMap = new int[result.width, result.depth];

        foreach (MapGenerationPass pass in settings.passes)
            pass.Execute(result);

        return result;
    }
}
