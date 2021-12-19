using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class NetworkObject : MonoBehaviour
{
    public abstract NetworkObjectType GetClassID();

    public virtual void Serialize(BinaryWriter writer) 
    {
    }

    public virtual void Deserialize(BinaryReader reader)
    {
    }
}
