using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateHeightMap : MapGenerationPass
{
    [Range(0, 1000)]
    public float NoiseScale = 250;
    [Range(0, 10)]
    public int Octaves = 5;
    [Range(0, 1)]
    public float Persistance = 0.5f;
    [Range(1, 10)]
    public float Lacunarity = 2;

    public Vector2 Offset;

    public override void Execute(MapData data)
    {
        data.heightMap = PerlinNoise.GenerateMap(
            data.seed, 
            data.width, 
            data.depth, 
            NoiseScale, 
            Octaves, 
            Persistance, 
            Lacunarity, 
            Offset);
    }
}
