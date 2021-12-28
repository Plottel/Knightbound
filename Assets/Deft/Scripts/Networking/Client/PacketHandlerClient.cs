using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Deft.Networking
{
    public abstract class PacketHandlerClient
    {
        public abstract void HandlePacket(string originIP, BinaryReader reader);
    }
}
