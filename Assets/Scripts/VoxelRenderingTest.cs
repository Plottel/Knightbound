using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelRenderingTest : MonoBehaviour
{
    Mesh mesh;

    List<Vector3> vertices;
    List<int> triangles;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void Start()
    {
        GenerateMeshData();
        ConstructMesh();
    }

    void GenerateMeshData()
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();

        MakeCube();
    }

    void MakeQuad(Vector3 position)
    {
        vertices.AddRange(VoxelHelper.GetQuadVertices(position));

        // Add the most recent 6 indexes of vertices to triangles
        int lastIndex = vertices.Count - 1;
        triangles.Add(lastIndex - 5);
        triangles.Add(lastIndex - 4);
        triangles.Add(lastIndex - 3);
        triangles.Add(lastIndex - 2);
        triangles.Add(lastIndex - 1);
        triangles.Add(lastIndex);
    }

    void MakeCube()
    {
        for (int i = 0; i < 6; ++i)
            MakeCubeFace((VoxelDirection)i);
    }

    void MakeCubeFace(VoxelDirection direction)
    {
        // Fetch 4 vertices and 6 Triangles
        vertices.AddRange(VoxelHelper.GetFaceVertices4Verts(direction));

        // Add the most recent 4 indexes of vertices to triangles
        int lastIndex = vertices.Count - 1;
        //triangles.Add(lastIndex - 5);
        //triangles.Add(lastIndex - 4);
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

        foreach (Vector3 v in VoxelHelper.GetCubeVertexOffsets())
            Gizmos.DrawSphere(v, 0.1f);
    }
}
