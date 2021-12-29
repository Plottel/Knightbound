using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deft.Networking
{
    public enum NetworkState
    {
        Uninitialized,
        Connecting,
        Connected,
        Welcomed,
        Playing
    }
}
