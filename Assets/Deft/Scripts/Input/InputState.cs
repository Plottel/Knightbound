using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Deft
{
    public class InputState
    {
        public bool move;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(move);
        }

        public void Deserialize(BinaryReader reader)
        {
            move = reader.ReadBoolean();
        }

    }
}