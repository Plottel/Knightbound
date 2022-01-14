using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterData))]
public class CharacterDataEditor : Editor
{
    private string meshPrefix;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharacterData characterData = target as CharacterData;

        if (GUILayout.Button("Map Default Bone IDs"))
            MapDefaultBoneIDs(characterData);

        meshPrefix = EditorGUILayout.TextField("Mesh Prefix", meshPrefix);

        GUILayout.Space(10);

        RenderBoneNames(characterData.armatureData);
    }

    void MapDefaultBoneIDs(CharacterData characterData)
    {
        // Try to find a matching Bone for each Mesh via name substitution:
        // e.g. Mesh "Character_Torso" -> "Torso" Bone
        foreach (CharacterMeshPiece meshPiece in characterData.defaultMeshPieces)
        {
            string boneName = meshPiece.mesh.name.Replace(meshPrefix, "");
            boneName = boneName.Replace('_', '.');

            meshPiece.boneID = characterData.armatureData.GetBoneID(boneName);
        }
    }

    void RenderBoneNames(ArmatureData armatureData)
    {
        if (armatureData == null || armatureData.boneNames == null)
            return;

        GUILayout.Label("Bone IDs", EditorStyles.boldLabel);

        string[] boneNames = armatureData.boneNames;
        for (int i = 0; i < boneNames.Length; ++i)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(i + " - " + boneNames[i]);
            GUILayout.EndHorizontal();
        }
    }
}
