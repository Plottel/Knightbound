using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Sprites;
using UnityEngine.U2D;

[CustomEditor(typeof(VoxelRenderingTest))]
public class VoxelRenderingTestEditor : Editor
{
    VoxelRenderingTest test;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        test = target as VoxelRenderingTest;

        if (GUILayout.Button("Pack Block Textures"))
            PackBlockTextures();
    }

    void PackBlockTextures()
    {
        test.blockTextureAtlas = new Texture2D(4096, 4096);
        test.blockTextureAtlas.filterMode = FilterMode.Point;
        test.blockTextureUVs = test.blockTextureAtlas.PackTextures(test.blockTextures, 0, 4096);
    }

    void SetMaterialTexture()
    {
        var test = target as VoxelRenderingTest;
        var renderer = test.GetComponent<MeshRenderer>();

        

        // Set Material Texture
        //renderer.material.mainTexture = atlasTexture;
    }
}
