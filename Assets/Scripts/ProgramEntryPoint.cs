using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;

#if UNITY_EDITOR
using ParrelSync;
#endif

public class ProgramEntryPoint : Manager<ProgramEntryPoint>
{
    public GameResources GameResources;

    void Awake()
    {
        EnsureInstance();

        // Setup Game Resources
        GameResources.EnsureInstance();
        GameResources.OnAwake();

#if UNITY_EDITOR
        if (ClonesManager.IsClone())
            InitializeClient();
        else
        {
            InitializeServer();
            InitializeClient();
        }
#else // In a Build...
        InitializeClient();
#endif
    }

    private void Start()
    {
        Destroy(gameObject);
    }

    public void InitializeServer()
    {
        GameAdmin server = SpawnGameAdmin("Server");

        // Data Singletons
        server.AddManager<InputBufferServer>();

        // Managers
        server.AddManager<ServerEntryPoint>();
        server.AddManager<NetworkManagerServer>();
        server.AddManager<PlayerManagerServer>();
        server.AddManager<ReceivedInputProcessorServer>();
        server.AddManager<CharacterMovementSystem>();
        server.AddManager<InputDispatcherServer>();
        server.AddManager<ReplicationManagerServer>();
        
        // Nothing-to-do-with-Networking
        server.AddManager<WorldManagerServer>();

        // Finalize
        server.NotifyManagersAwake();
    }

    public void InitializeClient()
    {
        GameAdmin client = SpawnGameAdmin("Client");

        // Data Singletons
        client.AddManager<UserInputSettings>();
        client.AddManager<InputBufferClient>();
        client.AddManager<UIView>();

        // Managers
        client.AddManager<GameStateManager>();
        client.AddManager<NetworkManagerClient>();
        client.AddManager<PlayerManagerClient>();
        client.AddManager<InputGeneratorClient>();
        client.AddManager<LocalInputProcessor>();
        client.AddManager<InputDispatcherClient>();
        client.AddManager<ReceivedInputProcessorClient>();
        client.AddManager<ReplicationManagerClient>();


        // "Nothing to do with Networking"-Managers
        client.AddManager<UIController>();
        client.AddManager<WorldManagerClient>();
        client.AddManager<CameraManager>();

        // Finalize
        client.NotifyManagersAwake();
    }

    GameAdmin SpawnGameAdmin(string name)
    {
        var admin = new GameObject().AddComponent<GameAdmin>();
        admin.name = name;

        return admin;
    }
}
