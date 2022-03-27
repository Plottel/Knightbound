using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Deft;

public class InputBufferClient : Manager<InputBufferClient>
{
    List<InputState> localBuffer; // Outgoing Input


    Dictionary<int, List<InputState>> playerIDToReceivedInputBuffer; // Incoming Input

    public override void OnAwake()
    {
        base.OnAwake();
        localBuffer = new List<InputState>();
        playerIDToReceivedInputBuffer = new Dictionary<int, List<InputState>>();
    }

    // Local Input
    public void AddLocalInputState(InputState state)
        => localBuffer.Add(state);

    public void ClearLocalBuffer()
        => localBuffer.Clear();

    public List<InputState> GetLocalBuffer()
        => localBuffer;

    public InputState GetLatestLocalInput()
        => localBuffer.Last();

    // Received Input
    public void AddReceivedInputState(InputState state)
    {
        if (playerIDToReceivedInputBuffer.TryGetValue(state.playerID, out var buffer))
            buffer.Add(state);
        else
            playerIDToReceivedInputBuffer.Add(state.playerID, new List<InputState> { state });
    }

    public void ClearReceivedBuffers()
        => playerIDToReceivedInputBuffer.Clear();

    public List<InputState> GetReceivedBuffer(int playerID)
        => playerIDToReceivedInputBuffer[playerID];

    public List<InputState>[] GetAllReceivedBuffers()
        => playerIDToReceivedInputBuffer.Values.ToArray();
}
