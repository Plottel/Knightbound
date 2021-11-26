using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ENet;

public static class NetworkUtils
{
    public static WelcomePacket CreateWelcomePacket(string message)
    {
        return new WelcomePacket { message = message, playerID = -1 };
    }

    public static ConsoleMessagePacket CreateConsoleMessagePacket(string message)
    {
        return new ConsoleMessagePacket { message = message };
    }
}
