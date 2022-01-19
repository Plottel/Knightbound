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

    public static MemoryStream MakeSpawnApprovedPacket()
    {
        MemoryStream stream = new MemoryStream();
        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
        {
            writer.Write((int)PacketType.Welcome);
            writer.Write((int)WelcomeMessage.SpawnApproved);
        }

        return stream;
    }

    public static MemoryStream MakeBeginPlayingApprovedPacket()
    {
        MemoryStream stream = new MemoryStream();
        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
        {
            writer.Write((int)PacketType.Welcome);
            writer.Write((int)WelcomeMessage.BeginPlayingApproved);
        }

        return stream;
    }

    public static MemoryStream MakeSetPlayerInfoPacket(int playerID, int characterID)
    {
        MemoryStream stream = new MemoryStream();
        using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
        {
            writer.Write((int)PacketType.SetPlayerInfo);
            writer.Write(playerID);
            writer.Write(characterID);
        }

        return stream;
    }
}
