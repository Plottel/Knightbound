using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public static class VoxelMeshGenerator
{
    static Mesh mesh;
    static List<Vector3> vertices;
    static List<int> triangles;
    static List<Vector2> uvs;
    static TextureAtlas atlas;

    public static VoxelMesh GenerateMesh(VoxelWorldData data, TextureAtlas textures)
    {
        // Init Data
        mesh = new Mesh();
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
        atlas = textures;

        GenerateMeshData(data);
        ApplyMeshData();
        
        VoxelMesh result = CreateMeshObject();

        // Clear Data
        vertices.Clear(); 
        triangles.Clear();
        uvs.Clear();
        atlas = null;
        mesh = null;

        return result;
    }

    static void GenerateMeshData(VoxelWorldData data)
    {
        for (int x = 0; x < data.Width; ++x)
        {
            for (int z = 0; z < data.Depth; ++z)
            {
                if (data.GetBlockType(x, z) != BlockType.Air)
                {
                    var blockPosition = new Vector3(x, 0, z);
                    AddCubeMesh(blockPosition, x, z, data);
                }
            }
        }
    }

    static void ApplyMeshData()
    {
        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    static void AddCubeMesh(Vector3 position, int x, int z, VoxelWorldData data)
    {
        for (int i = 0; i < 6; ++i)
        {
            var direction = (VoxelDirection)i;

            // Only draw faces facing Air.
            // To decouple, this should simply be fetching an indexed texture
            // If there is no texture (i.e. air), then we don't AddCubeFaceMesh
            if (data.GetNeighborBlockType(x, z, direction) == BlockType.Air)
                AddCubeFaceMesh((VoxelDirection)i, position, data.GetBlockType(x, z));
        }
    }

    static void AddCubeFaceMesh(VoxelDirection direction, Vector3 position, int blockType)
    {
        Vector3[] faceVertices = VoxelMeshUtils.GetFaceVertices6Verts(direction, position);

        // Fetch 6 vertices and 6 Triangles
        vertices.AddRange(faceVertices);

        // Add the most recent 6 indexes of vertices to triangles
        int lastIndex = vertices.Count - 1;
        triangles.Add(lastIndex - 5);
        triangles.Add(lastIndex - 4);
        triangles.Add(lastIndex - 3);
        triangles.Add(lastIndex - 2);
        triangles.Add(lastIndex - 1);
        triangles.Add(lastIndex);

        // Set UVs
        // 0-1-2 and 2-1-3 are the 2 triangles we draw from faceVertices indexes
        // TL, TR, BL, BL, TR, BR
        // Add 6 UVs that will correspond to 6 Vertices. 2 of these are repeated.
        // Each UV will need to be fetched from the texture atlas
        // Each U and V will be a number between 0 and 1

        // Assume Block type 0 (Air) never gets here.
        Rect uvRect = atlas.GetUVs(blockType - 1);

        Vector2 topLeft = new Vector2(uvRect.xMin, uvRect.yMax);
        Vector2 topRight = new Vector2(uvRect.xMax, uvRect.yMax);
        Vector2 bottomLeft = new Vector2(uvRect.xMin, uvRect.yMin);
        Vector2 bottomRight = new Vector2(uvRect.xMax, uvRect.yMin);

        // TL, TR, BL, BL, TR, BR
        uvs.Add(topLeft);
        uvs.Add(topRight);
        uvs.Add(bottomLeft);
        uvs.Add(bottomLeft);
        uvs.Add(topRight);
        uvs.Add(bottomRight);
    }

    static VoxelMesh CreateMeshObject()
    {
        VoxelMesh result = new GameObject("VoxelMesh").AddComponent<VoxelMesh>();
        result.SetMesh(mesh, atlas.GetTexture());

        return result;
    }
}
