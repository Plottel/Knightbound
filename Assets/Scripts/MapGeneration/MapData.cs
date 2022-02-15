using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: More Advanced Solution Required...
public struct LineSegment
{
    public Vector2 start;
    public Vector2 end;
}

public class MapData
{
    public int seed;
    public int width;
    public int depth;
    public Vector2Int spawn;
    public float[,] heightMap;
    public int[,] terrainMap;
    public LineSegment[] roads;
}
