using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Deft.Networking;

public static class PacketHelperServer
{
    public static MemoryStream MakeConnectionApprovedPacket(int playerID)
    {
        MemoryStream stream = new MemoryStream();
        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
        {
            writer.Write((int)PacketType.Welcome);
            writer.Write((int)WelcomeMessage.ConnectionApproved);
            writer.Write(playerID);
        }

        return stream;
    }

    public static MemoryStream MakeSpawnPacket(int playerID, int networkID)
    {
        MemoryStream stream = new MemoryStream();
        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
        {
            writer.Write((int)PacketType.Welcome);
            writer.Write((int)WelcomeMessage.Spawn);
            writer.Write(playerID);
            writer.Write(networkID);
        }

        return stream;
    }

    public static MemoryStream MakeBeginPlayingPacket()
    {
        MemoryStream stream = new MemoryStream();
        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
        {
            writer.Write((int)PacketType.Welcome);
            writer.Write((int)WelcomeMessage.BeginPlaying);
        }

        return stream;
    }
}
