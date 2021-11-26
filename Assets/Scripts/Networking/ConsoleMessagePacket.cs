using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConsoleMessagePacket : GamePacket
{
    public string message;

    public void SerializeHeader(BinaryWriter writer) { }
    public void DeserializeHeader(BinaryReader reader) { }

    public void SerializeBody(BinaryWriter writer)
    {
        writer.Write(message);
    }

    public void DeserializeBody(BinaryReader reader)
    {
        message = reader.ReadString();
    }


}
