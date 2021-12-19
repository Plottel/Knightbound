using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SmallManRigTest))]
public class SmallManRigTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var rig = target as SmallManRigTest;

        if (GUILayout.Button("Attach Meshes"))
            rig.AttachMeshes();

        
    }
}
