using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Deft;

public class InputBufferClient : Manager<InputBufferClient>
{
    List<InputState> localBuffer; // Outgoing Input
    List<InputState> receivedBuffer; // Incoming Input

    public override void OnAwake()
    {
        base.OnAwake();
        localBuffer = new List<InputState>();
        receivedBuffer = new List<InputState>();
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
        => receivedBuffer.Add(state);

    public void ClearReceivedBuffer()
        => receivedBuffer.Clear();

    public List<InputState> GetReceivedBuffer()
        => receivedBuffer;
}
