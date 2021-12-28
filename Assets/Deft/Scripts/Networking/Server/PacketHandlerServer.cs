using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Deft.Networking
{
    public abstract class PacketHandlerServer
    {
        public abstract void HandlePacket(uint peerID, BinaryReader reader);
    }
}
