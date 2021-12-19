using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PacketHeader
{
    public string originIP;
    public uint peerID;

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(originIP);
        writer.Write(peerID);
    }

    public void Deserialize(BinaryReader reader)
    {
        originIP = reader.ReadString();
        peerID = reader.ReadUInt32();
    }
}
