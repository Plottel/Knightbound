using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
