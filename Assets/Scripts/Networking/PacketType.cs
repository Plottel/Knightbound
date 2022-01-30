using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PacketType
{
    Welcome = 0,
    ConsoleMessage = 1,
    Replication = 2,
    Input = 3,
    SetPlayerInfo = 4,
    SetVoxelData = 5
}
