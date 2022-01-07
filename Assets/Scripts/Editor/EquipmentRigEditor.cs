using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EquipmentRig))]
public class EquipmentRigEditor : Editor
{
    private static Dictionary<string, string> meshSuffixToBoneName =
        new Dictionary<string, string>
        {
            { "Helmet",     "Head.end" },
            { "Boots_L",    "Shin.L.end" },
            { "Boots_R",    "Shin.R.end" },
            { "Leggings_L",    "Thigh.L" },
            { "Leggings_R",    "Thigh.R" },
            { "Chestplate",    "Torso" },
            { "Shoulder_L",    "Upper.Arm.L" },
            { "Shoulder_R",    "Upper.Arm.R" },
            { "Bracer_L",    "Lower.Arm.L" },
            { "Bracer_R",    "Lower.Arm.R" },
        };

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var rig = target as EquipmentRig;

        if (GUILayout.Button("Attach Meshes"))
            AttachMeshes(rig);

        if (GUILayout.Button("Destroy Meshes"))
            DestroyMeshes(rig);
    }

    private void AttachMeshes(EquipmentRig rig)
    {
        if (rig.meshData == null)
            return;

        DestroyMeshes(rig);

        foreach (Transform mesh in rig.meshData.transforms)
        {
            string meshSuffix = mesh.name.Replace(rig.meshPrefix, "");
            //meshSuffix = meshSuffix.Replace('_', '.');

            string boneName = "";
            if (!meshSuffixToBoneName.TryGetValue(meshSuffix, out boneName))
                Debug.Log("Can't find Bone Name for Mesh Suffix " + meshSuffix);

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

    private void DestroyMeshes(EquipmentRig rig)
    {
        if (rig.meshes == null)
            return;

        for (int i = rig.meshes.Count - 1; i >= 0; --i)
            DestroyImmediate(rig.meshes[i].gameObject);

        rig.meshes.Clear();
    }
}
