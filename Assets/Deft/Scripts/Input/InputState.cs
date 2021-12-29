using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Deft
{
    public class InputState
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(up);
            writer.Write(down);
            writer.Write(left);
            writer.Write(right);
        }

        public void Deserialize(BinaryReader reader)
        {
            up = reader.ReadBoolean();
            down = reader.ReadBoolean();
            left = reader.ReadBoolean();
            right = reader.ReadBoolean();
        }
    }
}