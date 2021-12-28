using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Deft.Networking
{
    public abstract class NetworkObject : MonoBehaviour
    {
        public abstract int GetClassID();

        public virtual void Serialize(BinaryWriter writer)
        {
        }

        public virtual void Deserialize(BinaryReader reader)
        {
        }
    }
}
