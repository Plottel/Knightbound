using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Deft;
using Deft.Networking;

public static class PacketHelperClient
{
    public static MemoryStream MakeWelcomePacket(WelcomeMessage message)
    {
        MemoryStream stream = new MemoryStream();
        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
        {
            writer.Write((int)PacketType.Welcome);
            writer.Write((int)message);
        }

        return stream;
    }

    public static MemoryStream MakeConsoleMessagePacket(string message)
    {
        MemoryStream stream = new MemoryStream();
        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
        {
            writer.Write((int)PacketType.ConsoleMessage);
            writer.Write(message);
        }

        return stream;
    }

    public static MemoryStream MakeInputPacket(InputState inputState)
    {
        MemoryStream stream = new MemoryStream();
        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
        {
            writer.Write((int)PacketType.Input);
            inputState.Serialize(writer);
        }

        return stream;
    }
}
