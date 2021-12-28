using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deft.Networking
{
    public enum PacketType
    {
        Welcome = 0,
        ConsoleMessage = 1,
        Replication = 2,
        Input = 3
    }
}
