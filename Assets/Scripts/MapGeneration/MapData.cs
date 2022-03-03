using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapData
{
    public string name;
    public int seed;
    public int width;
    public int depth;
    public Vector2Int spawn;
    public float[,] heightMap;
    public int[,] terrainMap;
    public LineSegment[] roads;
}
