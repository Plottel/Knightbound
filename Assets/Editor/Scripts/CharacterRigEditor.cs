using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterRig))]
public class CharacterRigEditor : Editor
{
    TransformCache meshAssets = null;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharacterRig rig = target as CharacterRig;

        meshAssets = EditorGUILayout.ObjectField(meshAssets, typeof(TransformCache), false) as TransformCache;

        if (GUILayout.Button("Attach Meshes"))
        {
            if (meshAssets != null)
                rig.AttachMeshes(meshAssets.transforms);
        }

        if (GUILayout.Button("Destroy Meshes"))
            rig.DestroyMeshes();       
    }
}
