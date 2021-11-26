using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReplicationPacket : GamePacket
{
    public ReplicationAction action;
    public int networkID;
    public NetworkObjectType classID;
    public NetworkObject obj; // Bad. Think in STREAMS not Packet Objects

    public void SerializeHeader(BinaryWriter writer) 
    {
        writer.Write((int)action);
        writer.Write(networkID);
        writer.Write((int)classID);
    }

    public void DeserializeHeader(BinaryReader reader)
    {
        action = (ReplicationAction)reader.ReadInt32();
        networkID = reader.ReadInt32();
        classID = (NetworkObjectType)reader.ReadInt32();
    }

    public void SerializeBody(BinaryWriter writer)
    {
        if (obj != null)
            obj.Serialize(writer);
    }

    public void DeserializeBody(BinaryReader reader)
    {
        if (obj != null)
            obj.Deserialize(reader);
    }
}
