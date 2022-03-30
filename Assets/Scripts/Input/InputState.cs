using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputState
{
    public int playerID;
    public Vector2 movement;
    public bool cameraLeft;
    public bool cameraRight;

    public void Serialize(BinaryWriter writer)
    {
        writer.Write(playerID);

        writer.Write(movement.x);
        writer.Write(movement.y);

        writer.Write(cameraLeft);
        writer.Write(cameraRight);
    }

    public void Deserialize(BinaryReader reader)
    {
        playerID = reader.ReadInt32();

        movement = new Vector2
        {
            x = reader.ReadSingle(),
            y = reader.ReadSingle()
        };

        cameraLeft = reader.ReadBoolean();
        cameraRight = reader.ReadBoolean();
    }
}