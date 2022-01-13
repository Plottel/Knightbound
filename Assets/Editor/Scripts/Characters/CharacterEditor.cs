using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Character))]
public class CharacterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Character character = target as Character;

        if (GUILayout.Button("Attach Meshes"))
            character.AttachMeshes();

        if (GUILayout.Button("Destroy Meshes"))
            character.DestroyMeshes();
    }
}
