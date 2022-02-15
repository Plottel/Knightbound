using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int size = 1;
    public int seed = 1;

    [Range(0, 50)]
    public float NoiseScale;
    [Range(0, 10)]
    public int Octaves;
    [Range(0, 1)]
    public float Persistance;
    [Range(1, 10)]
    public float Lacunarity;

    public Vector2 Offset;

    public MapData GenerateMapData()
    {
        var result = new MapData();

        result.width = size;
        result.depth = size;
        result.heightMap = PerlinNoise.GenerateMap(seed, size, size, NoiseScale, Octaves, Persistance, Lacunarity, Offset);

        return result;
    }
}
