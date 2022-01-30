using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParrelSync;
using Deft;

public class ProgramEntryPoint : MonoBehaviour
{
    public GameResources GameResources;

    void Awake()
    {
        // Setup Game Resources
        GameResources.EnsureInstance();
        GameResources.OnAwake();

        if (ClonesManager.IsClone())
            InitializeClient();
        else
        {
            InitializeServer();
            InitializeClient();
        }
    }

    private void Start()
    {
        Destroy(gameObject);
    }

    void InitializeServer()
    {
        GameAdmin server = SpawnGameAdmin("Server");

        // Data Singletons
        server.AddManager<InputBufferServer>();

        // Managers
        server.AddManager<GameManagerServer>();
        server.AddManager<NetworkManagerServer>();
        server.AddManager<PlayerManagerServer>();
        server.AddManager<ReceivedInputProcessorServer>();
        server.AddManager<InputDispatcherServer>();
        server.AddManager<ReplicationManagerServer>();
        
        // Nothing-to-do-with-Networking
        server.AddManager<VoxelManagerServer>();

        // Finalize
        server.NotifyManagersAwake();
    }

    void InitializeClient()
    {
        GameAdmin client = SpawnGameAdmin("Client");

        // Data Singletons
        client.AddManager<UserInputSettings>();
        client.AddManager<InputBufferClient>();

        // Managers
        client.AddManager<GameManagerClient>();
        client.AddManager<NetworkManagerClient>();
        client.AddManager<PlayerManagerClient>();
        client.AddManager<InputGeneratorClient>();
        client.AddManager<LocalInputProcessor>();
        client.AddManager<InputDispatcherClient>();
        client.AddManager<ReceivedInputProcessorClient>();
        client.AddManager<ReplicationManagerClient>();

        // "Nothing to do with Networking"-Managers
        client.AddManager<VoxelManagerClient>();
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
