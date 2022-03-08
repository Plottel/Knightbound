using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Deft.Networking
{
    public abstract class NetworkObject : MonoBehaviour
    {
        public abstract int GetClassID();
        public virtual bool ShouldSendUpdate() => true;

        public abstract void Serialize(BinaryWriter writer);
        public abstract void Deserialize(BinaryReader reader);
    }
}
