using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;
using Deft.Networking;

public class GameResources : Manager<GameResources>
{
    // TODO: Move out of here. Consider ConnectionSettings like UserInputSettings ... ?
    public const string hostName = "127.0.0.1";
    public const ushort port = 6005;

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
