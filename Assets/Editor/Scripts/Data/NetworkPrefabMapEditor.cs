using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Deft.Networking;

[CustomEditor(typeof(NetworkPrefabMap))]
public class NetworkPrefabMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Fetch Prefabs"))
            FetchPrefabs();
    }

    void FetchPrefabs()
    {
        NetworkPrefabMap map = target as NetworkPrefabMap;
        int prefabCount = Enum.GetValues(typeof(PrefabID)).Length;

        map.prefabs = new NetworkObject[prefabCount];

        // For each Enum Value
        for (int i = 0; i < prefabCount; ++i)
        {
            string typeName = ((PrefabID)i).ToString();

            // Fetch all Asset GUIDs containing the Enum Value name
            string[] guids = AssetDatabase.FindAssets(typeName + " t:prefab", new string[] { "Assets/Prefabs" });

            if (guids.Length == 0)
            {
                Debug.LogError("No Prefabs found for " + typeName);
                continue;
            }

            // Convert all returned GUIDs to Paths
            string[] paths = GUIDsToPaths(guids);

            // Strip paths, searching for Asset that matches name exactly.
            string path = "";

            for (int j = 0; j < paths.Length; ++j)
            {
                string assetName = paths[j].Substring(paths[j].LastIndexOf('/') + 1);
                assetName = assetName.Replace(".prefab", "");

                if (assetName == typeName)
                {
                    path = paths[j];
                    break;
                }
            }

            if (path == "")
            {
                Debug.LogError("No Prefab found exactly matching name " + typeName);
                continue;
            }

            map.prefabs[i] = AssetDatabase.LoadAssetAtPath<NetworkObject>(path);
        }
    }

    string[] GUIDsToPaths(string[] guids)
    {
        string[] paths = new string[guids.Length];
        for (int i = 0; i < guids.Length; ++i)
            paths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);

        return paths;
    }
}
