using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputState
{
    public int playerID;
    public bool up;
    public bool down;
    public bool left;
    public bool right;
    public bool cameraLeft;
    public bool cameraRight;

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(playerID);
        writer.Write(up);
        writer.Write(down);
        writer.Write(left);
        writer.Write(right);
        writer.Write(cameraLeft);
        writer.Write(cameraRight);
    }

    public void Deserialize(BinaryReader reader)
    {
        playerID = reader.ReadInt32();
        up = reader.ReadBoolean();
        down = reader.ReadBoolean();
        left = reader.ReadBoolean();
        right = reader.ReadBoolean();
        cameraLeft = reader.ReadBoolean();
        cameraRight = reader.ReadBoolean();
    }
}