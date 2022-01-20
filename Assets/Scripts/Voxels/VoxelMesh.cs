using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class VoxelMesh : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetMesh(Mesh newMesh, Texture2D texture)
    {
        meshFilter.sharedMesh = newMesh;
        meshCollider.sharedMesh = newMesh;
        meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
