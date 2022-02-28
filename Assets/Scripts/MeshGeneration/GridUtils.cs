using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridUtils
{
    private static Vector3Int[] kNeighbourOffsets;

    static GridUtils()
    {
        kNeighbourOffsets = new Vector3Int[Enum.GetValues(typeof(GridDirection)).Length];
        kNeighbourOffsets[(int)GridDirection.Up] = new Vector3Int(0, 1, 0);
        kNeighbourOffsets[(int)GridDirection.Down] = new Vector3Int(0, -1, 0);
        kNeighbourOffsets[(int)GridDirection.Left] = new Vector3Int(-1, 0, 0);
        kNeighbourOffsets[(int)GridDirection.Right] = new Vector3Int(1, 0, 0);
        kNeighbourOffsets[(int)GridDirection.Front] = new Vector3Int(0, 0, 1);
        kNeighbourOffsets[(int)GridDirection.Back] = new Vector3Int(0, 0, -1);
    }

    public static int GetValue(int[,] data, int x, int y, int z)
    {
        // TODO: Potential bottleneck of fetching 2 variables for every time we check an index
        int width = data.GetLength(0);
        int depth = data.GetLength(1);

        if (x < 0 || x >= width || z < 0 || z >= depth || y != 0)
            return 0;
        return data[x, z];
    }

    public static int GetValue(int[,] data, Vector3Int index)
        => GetValue(data, index.x, index.y, index.z);


    public static int GetNeighbourValue(int[,] data, int x, int y, int z, GridDirection neighbourDirection)
    {
        Vector3Int neighbourOffset = kNeighbourOffsets[(int)neighbourDirection];
        return GetValue(data, new Vector3Int(x, y, z) + neighbourOffset);
    }
}
