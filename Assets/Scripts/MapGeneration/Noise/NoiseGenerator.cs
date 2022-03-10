using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class NoiseGenerator
{
    public int SeedOffset;

    public abstract float[,] GenerateMap(int seed, int width, int depth);
}
