using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

[CustomEditor(typeof(MapGenerationTest))]
public class MapGenerationTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var tester = target as MapGenerationTest;

        if (GUILayout.Button("Generate Map"))
            tester.UpdateMap();
    }
}
