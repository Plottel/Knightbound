using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class PacketHandler
{
    public abstract void HandlePacket(string originIP, BinaryReader reader);
}
