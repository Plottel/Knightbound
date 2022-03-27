using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Deft;

public class InputBufferServer : Manager<InputBufferServer>
{
    Dictionary<int, List<InputState>> playerIDToInputBuffer;

    public override void OnAwake()
    {
        base.OnAwake();
        playerIDToInputBuffer = new Dictionary<int, List<InputState>>();
    }

    public void AddInputState(InputState state)
    {
        if (playerIDToInputBuffer.TryGetValue(state.playerID, out var buffer))
            buffer.Add(state);
        else
            playerIDToInputBuffer.Add(state.playerID, new List<InputState> { state });
    }

    public void ClearBuffers()
        => playerIDToInputBuffer.Clear();

    public List<InputState> GetBuffer(int playerID)
        => playerIDToInputBuffer[playerID];

    public List<InputState>[] GetAllBuffers()
        => playerIDToInputBuffer.Values.ToArray();
}

