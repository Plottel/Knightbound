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

    public Canvas CanvasTemplate;
    public UIPanelCache GamePanels;

    public NetworkObjectMap NetworkPrefabs;
    public Texture2D[] BlockTextures;
    [HideInInspector] public TextureAtlas BlockAtlas;

    public MapGenerationSettings MapGenerationSettings;
    public MapFabricationSettings MapFabricationSettings;

    public override void OnAwake()
    {
        base.OnAwake();

        // TODO: Refactor to common NetworkManager class that initializes this.
        // Setup Networking
        ENet.Library.Initialize();

        // Setup Network Prefabs
        var prefabs = NetworkPrefabs.prefabs;
        NetworkPrefabRegistry.SetPrefabCount(prefabs.Length);

        // Register Netowrk Prefabs
        for (int i = 0; i < prefabs.Length; i++)
            NetworkPrefabRegistry.Register(i, prefabs[i]);

        // TODO: Clean this up with a custom editor to pack the textures in Edit Mode.
        // Create Block Atlas
        BlockAtlas = new TextureAtlas(BlockTextures);
    }

    private void OnDestroy()
    {
        ENet.Library.Deinitialize();
    }
}
