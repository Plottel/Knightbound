using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Deft;
using Deft.Networking;

public static class PacketHelperClient
{
    public static MemoryStream MakeWelcomePacket(WelcomeMessage message, int playerID)
    {
        MemoryStream stream = new MemoryStream();
        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
        {
            writer.Write((int)PacketType.Welcome);
            writer.Write((int)message);
            writer.Write(playerID);
        }

        return stream;
    }

    public static MemoryStream MakeRequestConnectionPacket()
    {
        MemoryStream stream = new MemoryStream();
        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
        {
            writer.Write((int)PacketType.Welcome);
            writer.Write((int)WelcomeMessage.RequestConnection);
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

    public static MemoryStream MakeInputPacket(int playerID, InputState inputState)
    {
        MemoryStream stream = new MemoryStream();
        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
        {
            writer.Write((int)PacketType.Input);
            writer.Write(playerID);
            inputState.Serialize(writer);
        }

        return stream;
    }
}
