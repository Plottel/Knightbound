using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterRig))]
public class CharacterRigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var rig = target as CharacterRig;

        if (GUILayout.Button("Attach Meshes"))
            AttachMeshes(rig);

        if (GUILayout.Button("Destroy Meshes"))
            DestroyMeshes(rig);
    }

    private void AttachMeshes(CharacterRig rig)
    {
        if (rig.meshData == null)
            return;

        DestroyMeshes(rig);

        foreach (Transform mesh in rig.meshData.transforms)
        {
            string boneName = mesh.name.Replace(rig.meshPrefix, "");
            boneName = boneName.Replace('_', '.');

            if (rig.transform.TryFindTransform(boneName, out Transform bone))
            {
                Transform meshInstance = Instantiate(mesh);

                meshInstance.parent = bone.transform;
                meshInstance.position = bone.position;

                rig.meshes.Add(meshInstance);
            }
            else
                Debug.Log("Cannot find bone called " + boneName);
        }
    }

    private void DestroyMeshes(CharacterRig rig)
    {
        if (rig.meshes == null)
            return;

        for (int i = rig.meshes.Count - 1; i >= 0; --i)
            DestroyImmediate(rig.meshes[i].gameObject);

        rig.meshes.Clear();
    }
}
