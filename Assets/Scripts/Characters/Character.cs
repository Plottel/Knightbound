using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData characterData;
    public CharacterMesh characterMesh;

    public void AttachMeshes()
    {
        ArmatureData armatureData = characterData.armatureData;
        Transform rootBone = Instantiate(characterData.armature);
        CharacterMeshPiece[] meshPieces = characterData.defaultMeshPieces;

        characterMesh.SetRootBone(rootBone);

        foreach (CharacterMeshPiece meshPiece in meshPieces)
        {
            Transform mesh = Instantiate(meshPiece.mesh);

            if (armatureData.TryGetBoneName(meshPiece.boneID, out string boneName))
                characterMesh.AttachMeshPiece(mesh, boneName);
            else
                Debug.Log("Invalid Bone ID " + meshPiece.boneID);
        }
    }

    public void DestroyMeshes()
    {
        characterMesh.DestroyMeshes();
    }

    private void OnDrawGizmos()
    {
        string[] boneNames = characterData.armatureData.boneNames;

        foreach (string boneName in boneNames)
        {
            if (characterMesh.transform.TryFindTransform(boneName, out Transform bone))
            {
                Gizmos.DrawSphere(bone.position, 0.1f);
            }
        }
    }
}
