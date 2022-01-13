using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRig : MonoBehaviour
{
    public string meshPrefix;

    private List<Transform> meshInstances;

    void Awake()
    {
        meshInstances = new List<Transform>();
    }

    void OnReset()
    {
        meshInstances = new List<Transform>();
    }

    public void AttachMeshes(Transform[] meshAssets)
    {
        DestroyMeshes();

        // Try to find a matching Bone for each Mesh via name substitution:
        // e.g. Mesh "Character_Torso" -> "Torso" Bone
        foreach (Transform meshPrefab in meshAssets)
        {
            string boneName = meshPrefab.name.Replace(meshPrefix, "");
            boneName = boneName.Replace('_', '.');

            if (transform.TryFindTransform(boneName, out Transform bone))
            {
                Transform meshInstance = Instantiate(meshPrefab);
                meshInstance.parent = bone.transform;
                meshInstance.position = bone.position;

                meshInstances.Add(meshInstance);
            }
            else
                Debug.Log("Cannot find bone called " + boneName);
        }
    }

    public void DestroyMeshes()
    {
        for (int i = meshInstances.Count - 1; i >= 0; --i)
            DestroyImmediate(meshInstances[i].gameObject);

        meshInstances.Clear();
    }
}