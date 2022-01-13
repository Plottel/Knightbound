using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMesh : MonoBehaviour
{
    private Transform rootBone;

    [SerializeField][HideInInspector]
    private List<Transform> meshPieces;

    void Reset()
    {
        meshPieces = new List<Transform>();
    }

    public void SetRootBone(Transform root)
    {
        rootBone = root;
        rootBone.parent = transform;
        rootBone.position = transform.position;
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

    public void DestroyMeshes()
    {
        DestroyImmediate(rootBone.gameObject);
        meshPieces.Clear();
        //for (int i = meshPieces.Count - 1; i >= 0; --i)
        //    DestroyImmediate(meshPieces[i].gameObject);

        //meshPieces.Clear();
    }
}
