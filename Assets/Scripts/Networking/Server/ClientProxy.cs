using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

namespace Deft.Networking
{
    public class ClientProxy
    {
        public uint peerID;
        public int playerID;

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
}