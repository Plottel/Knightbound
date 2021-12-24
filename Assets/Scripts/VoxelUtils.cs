using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelUtils
{
    private static Vector3[] quadVertices = new Vector3[]
    {
        new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0),
        new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 1)
    };

    private static Vector3[] centeredQuadVertices = new Vector3[]
    {
        new Vector3(-0.5f, 0, -0.5f), new Vector3(-0.5f, 0, 0.5f), new Vector3(0.5f, 0, -0.5f),
        new Vector3(0.5f, 0, -0.5f), new Vector3(-0.5f, 0, 0.5f), new Vector3(0.5f, 0, 0.5f)
    };

    private static int[] quadTriangles = new int[]
    {
        0, 1, 2, 3, 4, 5
    };

    public static Vector3[] GetQuadVertices(Vector3 position)
    {
        Vector3[] result = new Vector3[quadVertices.Length];
        for (int i = 0; i < quadVertices.Length; ++i)
            result[i] = centeredQuadVertices[i] + position;

        return result;
    }

    private static Vector3[] cubeVertexOffsets = new Vector3[]
    {
        new Vector3(-0.5f, 0.5f, 0.5f),     // Left Top Front
        new Vector3(0.5f, 0.5f, 0.5f),      // Right Top Front
        new Vector3(-0.5f, 0.5f, -0.5f),    // Left Top Back
        new Vector3(0.5f, 0.5f, -0.5f),     // Right Top Back
        new Vector3(-0.5f, -0.5f, 0.5f),    // Left Bottom Front
        new Vector3(0.5f, -0.5f, 0.5f),     // Right Bottom Front
        new Vector3(-0.5f, -0.5f, -0.5f),   // Left Bottom Back
        new Vector3(0.5f, -0.5f, -0.5f)     // Right Bottom Back
    };

    // 4 verts is 0, 1, 2, 3
    private static int[][] faceTriangles4Verts = new int[][]
    {
        new int[] {0, 1, 3, 2},     // Top
        new int[] {4, 5, 7, 6},     // Bottom
        new int[] {0, 2, 6, 4},     // Left
        new int[] {1, 3, 7, 5},     // Right
        new int[] {0, 1, 5, 4},     // Front
        new int[] {2, 3, 7, 6}      // Back
    };

    // 6 verts is 0, 1, 2 and 2, 1, 3
    private static int[][] faceTriangles6Verts = new int[][]
    {
        new int[] {0, 1, 2, 2, 1, 3},     // Top
        new int[] {4, 6, 5, 5, 6, 7},     // Bottom  
        new int[] {0, 2, 4, 4, 2, 6},     // Left
        new int[] {1, 5, 3, 3, 5, 7},     // Right
        new int[] {0, 4, 1, 1, 4, 5},     // Front
        new int[] {2, 3, 6, 6, 3, 7}      // Back
    };

    public static Vector3[] GetCubeVertexOffsets()
    {
        Vector3[] result = new Vector3[8];
        for (int i = 0; i < 8; ++i)
            result[i] = cubeVertexOffsets[i];

        return result;
    }

    public static Vector3[] GetFaceVertices4Verts(VoxelDirection direction, Vector3 position)
    {
        Vector3[] result = new Vector3[4];
        for (int i = 0; i < 4; ++i)
            result[i] = cubeVertexOffsets[faceTriangles4Verts[(int)direction][i]] + position;

        return result;
    }

    public static Vector3[] GetFaceVertices6Verts(VoxelDirection direction, Vector3 position)
    {
        Vector3[] result = new Vector3[6];
        for (int i = 0; i < 6; ++i)
            result[i] = cubeVertexOffsets[faceTriangles6Verts[(int)direction][i]] + position;

        return result;
    }
}
