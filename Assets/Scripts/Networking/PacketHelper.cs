using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Deft;
using Deft.Networking;

public static class PacketHelper
{
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
