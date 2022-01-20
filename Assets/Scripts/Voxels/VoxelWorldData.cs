using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VoxelWorldData
{
    private static Vector3Int[] kNeighbourOffsets;

    public int[,] voxelData;

    public int Width => voxelData.GetLength(0);
    public int Depth => voxelData.GetLength(1);

    static VoxelWorldData()
    {
        kNeighbourOffsets = new Vector3Int[Enum.GetValues(typeof(VoxelDirection)).Length];
        kNeighbourOffsets[(int)VoxelDirection.Up] =         new Vector3Int(0, 1, 0);
        kNeighbourOffsets[(int)VoxelDirection.Down] =       new Vector3Int(0, -1, 0);
        kNeighbourOffsets[(int)VoxelDirection.Left] =       new Vector3Int(-1, 0, 0);
        kNeighbourOffsets[(int)VoxelDirection.Right] =      new Vector3Int(1, 0, 0);
        kNeighbourOffsets[(int)VoxelDirection.Front] =      new Vector3Int(0, 0, 1);
        kNeighbourOffsets[(int)VoxelDirection.Back] =       new Vector3Int(0, 0, -1);
    }

    public int GetBlockType(int x, int z)
    {
        if (x < 0 || x >= Width || z < 0 || z >= Depth)
            return 0;
        return voxelData[x, z];
    }

    public int GetBlockType(Vector3Int index)
    {
        if (index.x < 0 || index.x >= Width || index.z < 0 || index.z >= Depth || index.y != 0)
            return 0;
        return voxelData[index.x, index.z];
    }

    public int GetNeighborBlockType(int x, int z, VoxelDirection direction)
    {
        Vector3Int neighbourOffset = kNeighbourOffsets[(int)direction];
        return GetBlockType(new Vector3Int(x, 0, z) + neighbourOffset);
    }

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(Width);
        writer.Write(Depth);

        for (int x = 0; x < Width; ++x)
        {
            for (int z = 0; z < Depth; ++z)
                writer.Write(voxelData[x, z]);
        }
    }

    public void Deserialize(BinaryReader reader)
    {
        int width = reader.ReadInt32();
        int depth = reader.ReadInt32();

        voxelData = new int[width, depth];

        for (int x = 0; x < Width; ++x)
        {
            for (int z = 0; z < Depth; ++z)
                voxelData[x, z] = reader.ReadInt32();
        }
    }
}
