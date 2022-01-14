using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data/ArmatureData", menuName = "ArmatureData")]
public class ArmatureData : ScriptableObject
{
    public string[] boneNames;

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
