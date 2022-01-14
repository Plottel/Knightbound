using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMesh : MonoBehaviour
{
    private Transform rootBone;
    private string[] boneNames;

    [SerializeField][HideInInspector]
    private List<Transform> meshPieces;

    void Reset()
    {
        meshPieces = new List<Transform>();
    }

    public void SetBoneData(Transform newRootBone, string[] newBoneNames)
    {
        DestroyRootBone();

        rootBone = newRootBone;
        rootBone.parent = transform;
        rootBone.position = transform.position;

        boneNames = newBoneNames;
    }

    public void AttachMeshPiece(Transform mesh, string boneName)
    {
        if (rootBone.TryFindTransform(boneName, out Transform bone))
        {
            mesh.parent = bone.transform;
            mesh.position = bone.position;

            meshPieces.Add(mesh);
        }
        else
            Debug.Log("Cannot find bone named " + boneName);
    }

    public void AttachMeshes(CharacterMeshPiece[] meshPieces)
    {
        DestroyMeshes();

        foreach (CharacterMeshPiece meshPiece in meshPieces)
        {
            Transform mesh = Instantiate(meshPiece.mesh);

            if (TryGetBoneName(meshPiece.boneID, out string boneName))
                AttachMeshPiece(mesh, boneName);
            else
                Debug.Log("Invalid Bone ID " + meshPiece.boneID);
        }
    }

    private bool TryGetBoneName(int boneID, out string boneName)
    {
        if (boneNames == null || boneID < 0 || boneID >= boneNames.Length)
        {
            Debug.Log("Failed to find Bone Name for ID " + boneID);
            Debug.Log("Bone Names Count" + boneNames[boneID]);
            boneName = "";
            return false;
        }

        boneName = boneNames[boneID];
        return true;
    }

    public void DestroyRootBone()
    {
        if (rootBone == null)
            return;

        DestroyImmediate(rootBone.gameObject);
        rootBone = null;
    }

    public void DestroyMeshes()
    {
        for (int i = meshPieces.Count - 1; i >= 0; --i)
            DestroyImmediate(meshPieces[i].gameObject);

        meshPieces.Clear();
    }
}
