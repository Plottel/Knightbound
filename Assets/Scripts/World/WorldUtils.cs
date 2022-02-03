using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldUtils
{
    private static Vector3Int[] kNeighbourOffsets;

    static WorldUtils()
    {
        kNeighbourOffsets = new Vector3Int[Enum.GetValues(typeof(GridDirection)).Length];
        kNeighbourOffsets[(int)GridDirection.Up] = new Vector3Int(0, 1, 0);
        kNeighbourOffsets[(int)GridDirection.Down] = new Vector3Int(0, -1, 0);
        kNeighbourOffsets[(int)GridDirection.Left] = new Vector3Int(-1, 0, 0);
        kNeighbourOffsets[(int)GridDirection.Right] = new Vector3Int(1, 0, 0);
        kNeighbourOffsets[(int)GridDirection.Front] = new Vector3Int(0, 0, 1);
        kNeighbourOffsets[(int)GridDirection.Back] = new Vector3Int(0, 0, -1);
    }

    public static int GetBlockType(WorldData data, Vector3Int index)
        => GetBlockType(data, index.x, index.y, index.z);

    public static int GetBlockType(WorldData data, int x, int y, int z)
    {
        if (x < 0 || x >= data.Width || z < 0 || z >= data.Depth || y != 0)
            return 0;
        return data.blocks[x, z];
    }

    public static int GetNeighborBlockType(WorldData data, int x, int y, int z, GridDirection direction)
    {
        Vector3Int neighbourOffset = kNeighbourOffsets[(int)direction];
        return GetBlockType(data, new Vector3Int(x, y, z) + neighbourOffset);
    }
}
