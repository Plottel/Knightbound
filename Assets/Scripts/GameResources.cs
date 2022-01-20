using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;
using Deft.Networking;

public class GameResources : Manager<GameResources>
{
    public NetworkObjectMap NetworkPrefabs;
    public Texture2D[] BlockTextures;

    public override void OnAwake()
    {
        base.OnAwake();

        // Setup Networking
        ENet.Library.Initialize();

        // Setup Network Prefabs
        var prefabs = NetworkPrefabs.prefabs;
        NetworkPrefabRegistry.SetPrefabCount(prefabs.Length);

        // Register Netowrk Prefabs
        for (int i = 0; i < prefabs.Length; i++)
            NetworkPrefabRegistry.Register(i, prefabs[i]);
    }

    private void OnDestroy()
    {
        ENet.Library.Deinitialize();
    }
}
