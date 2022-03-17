using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateProp : MapGenerationPass
{
    public PrefabID PropType;
    [SerializeReference] public NoiseGenerator Noise;
    public float NoiseRangeMin;
    public float NoiseRangeMax;

    public override void Execute(MapData data)
    {
        int propID = (int)PropType;

        float[,] noiseMap = Noise.GenerateMap(data.seed, data.width, data.depth);

        for (int x = 0; x < data.width; ++x)
        {
            for (int z = 0; z < data.depth; ++z)
            {
                float noise = noiseMap[x, z];
                if (noise >= NoiseRangeMin && noise <= NoiseRangeMax)
                    data.propMap[x, z] = propID;
            }
        }
    }
}
