using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Deft.Networking
{
    public class ReplicationCommand
    {
        public ReplicationAction action;
        public int networkID;
        public int classID;
        public BinaryReader reader;
    }
}
