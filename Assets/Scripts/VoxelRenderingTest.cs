using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VoxelRenderingTest : MonoBehaviour
{
    public Texture2D[] blockTextures;
    public Rect[] blockTextureUVs;

    public Texture2D blockTextureAtlas;
    public Vector3 worldOrigin;

    VoxelWorldData worldData;
    Mesh mesh;

    List<Vector3> vertices;
    List<int> triangles;
    List<Vector2> uvs;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        GetComponent<MeshRenderer>().sharedMaterial.mainTexture = blockTextureAtlas;
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
        uvs = new List<Vector2>();

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
                MakeCubeFace((VoxelDirection)i, position, data.GetBlockType(x, z));

        }
    }

    void MakeCubeFace(VoxelDirection direction, Vector3 position, int blockType)
    {
        Vector3[] faceVertices = VoxelUtils.GetFaceVertices6Verts(direction, position);

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
        Rect uvRect = blockTextureUVs[blockType - 1];

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

    void ConstructMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(Vector3.zero, 0.1f);

        foreach (Vector3 v in VoxelUtils.GetCubeVertexOffsets())
            Gizmos.DrawSphere(v, 0.1f);
    }
}
