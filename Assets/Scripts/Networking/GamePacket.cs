using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public interface GamePacket
{
    void SerializeHeader(BinaryWriter writer);
    void DeserializeHeader(BinaryReader reader);

    void SerializeBody(BinaryWriter writer);
    void DeserializeBody(BinaryReader reader);
}
