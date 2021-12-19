using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientProxy
{
    public WelcomeState state;
    public uint peerID;
    public int playerID;
    public int playerObjNetworkID;

    public NetworkReplicator replicator = new NetworkReplicator();

    private InputState inputState;

    public void AddInputState(InputState inputState)
    {
        this.inputState = inputState;
    }

    public bool GetInputState(out InputState state)
    {
        state = inputState;
        return state != null;
    }
}