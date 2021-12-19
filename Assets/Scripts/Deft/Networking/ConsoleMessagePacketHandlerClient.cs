using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConsoleMessagePacketHandlerClient : PacketHandlerClient
{
    public override void HandlePacket(string originIP, BinaryReader reader)
    {
        string message = reader.ReadString();

        // If receiving, it has already been sent to the server and distributed.
        // This is the final step to simply print it out.
        Debug.Log("Console Message: " + message);
    }
}
