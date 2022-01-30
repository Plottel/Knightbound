using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

public class InputBufferServer : Manager<InputBufferServer>
{
    List<InputState> buffer;

    public override void OnAwake()
    {
        base.OnAwake();
        buffer = new List<InputState>();
    }

    public void AddInputState(InputState state)
        => buffer.Add(state);

    public void ClearBuffer()
        => buffer.Clear();

    public List<InputState> GetBuffer()
        => buffer;
}

