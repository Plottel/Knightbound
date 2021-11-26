using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class WelcomePacket : GamePacket
{
    public string message;
    public int playerID = -1;

    public void SerializeHeader(BinaryWriter writer) { }
    public void DeserializeHeader(BinaryReader reader) { }

    public void SerializeBody(BinaryWriter writer)
    {
        writer.Write(message);
        writer.Write(playerID);
    }

    public void DeserializeBody(BinaryReader reader)
    {
        message = reader.ReadString();
        playerID = reader.ReadInt32();
    }
}
