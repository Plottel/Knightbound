using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelRenderingTest : MonoBehaviour
{
    public Vector3 worldOrigin;

    VoxelWorldData worldData;
    Mesh mesh;

    List<Vector3> vertices;
    List<int> triangles;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void Start()
    {
        worldData = new VoxelWorldData();
        GenerateMeshData();
        ConstructMesh();
    }

    void GenerateMeshData()
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();

        MakeVoxelWorld(worldData);

        //MakeCube(new Vector3(0, 0, 0));
        //MakeCube(new Vector3(1, 0, 0));
        //MakeCube(new Vector3(0, 0, 1));
        //MakeCube(new Vector3(1, 0, 1));
    }

    void MakeVoxelWorld(VoxelWorldData data)
    {
        for (int x = 0; x < data.Width; ++x)
        {
            for (int z = 0; z < data.Depth; ++z)
            {
                if (data.GetBlockType(x, z) != BlockType.Air)
                {
                    var blockPosition = worldOrigin + new Vector3(x, 0, z);
                    MakeCube(blockPosition, x, z, data);
                }
            }
        }
    }

    void MakeCube(Vector3 position, int x, int z, VoxelWorldData data)
    {
        for (int i = 0; i < 6; ++i)
        {
            var direction = (VoxelDirection)i;

            // Only draw faces facing Air.
            if (data.GetNeighborBlockType(x, z, direction) == BlockType.Air)
                MakeCubeFace((VoxelDirection)i, position);

        }
    }

    void MakeCubeFace(VoxelDirection direction, Vector3 position)
    {
        // Fetch 6 vertices and 6 Triangles
        vertices.AddRange(VoxelUtils.GetFaceVertices6Verts(direction, position));

        // Add the most recent 6 indexes of vertices to triangles
        int lastIndex = vertices.Count - 1;
        triangles.Add(lastIndex - 5);
        triangles.Add(lastIndex - 4);
        triangles.Add(lastIndex - 3);
        triangles.Add(lastIndex - 2);
        triangles.Add(lastIndex - 1);
        triangles.Add(lastIndex);
    }

    void ConstructMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(Vector3.zero, 0.1f);

        foreach (Vector3 v in VoxelUtils.GetCubeVertexOffsets())
            Gizmos.DrawSphere(v, 0.1f);
    }
}
