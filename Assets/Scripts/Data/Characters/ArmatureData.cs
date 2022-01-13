using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data/ArmatureData", menuName = "ArmatureData")]
public class ArmatureData : ScriptableObject
{
    public string[] boneNames;

    public bool TryGetBoneName(int boneID, out string boneName)
    {
        if (boneNames == null || boneID < 0 || boneID >= boneNames.Length)
        {
            boneName = "";
            return false;
        }

        boneName = boneNames[boneID];
        return true;
    }

    public bool TryGetBoneID(string boneName, out int boneID)
    {
        boneID = -1;

        if (boneNames == null)
            return false;

        for (int i = 0; i < boneNames.Length; ++i)
        {
            if (boneNames[i] == boneName)
            {
                boneID = i;
                return true;
            }
        }

        return false;
    }

    public int GetBoneID(string boneName)
    {
        if (boneNames == null)
            return -1;

        for (int i = 0; i < boneNames.Length; ++i)
        {
            if (boneNames[i] == boneName)
                return i;
        }

        return -1;
    }
}
