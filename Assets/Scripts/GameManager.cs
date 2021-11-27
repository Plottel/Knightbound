using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Deft.Networking;

public class GameManager : MonoBehaviour
{
    private string hostName = "127.0.0.1";
    private ushort port = 6005;

    private GameServer server;
    private GameClient client;

    int frameCount;
    int packetCount;

    Tree tree;

    void Awake()
    {
        ENet.Library.Initialize();

        server = new GameServer();
        client = new GameClient();
        client.replicator.context = server.replicator.context; // Local client - share context.

        server.SetPacketHandler<WelcomePacketHandlerServer>(PacketType.Welcome);
        server.SetPacketHandler<ConsoleMessagePacketHandlerServer>(PacketType.ConsoleMessage);

        client.SetPacketHandler<WelcomePacketHandlerClient>(PacketType.Welcome);
        client.SetPacketHandler<ConsoleMessagePacketHandlerClient>(PacketType.ConsoleMessage);
        client.SetPacketHandler<ReplicationPacketHandlerClient>(PacketType.Replication);

        server.LaunchServer(port);
        client.JoinServer(hostName, port);
    }

    void Update()
    {
        server.ProcessPackets();
        client.ProcessPackets();

        // Send Message on Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var packet = NetworkUtils.CreateConsoleMessagePacket("My First Message");
            client.SendPacket(PacketType.ConsoleMessage, packet);
        }

        // Spawn Tree and send Create packet on C
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Create a Tree on the server
            tree = NetworkPrefabRegistry.Create<Tree>(NetworkObjectType.Tree);
            tree.name = "IM A TREE!";

            int networkID = server.replicator.context.RegisterNewNetworkObject(tree);
            NetworkObjectType classID = NetworkObjectType.Tree;

            var packet = new ReplicationPacket();
            packet.action = ReplicationAction.Create;
            packet.classID = classID;
            packet.networkID = networkID;
            packet.obj = tree;

            server.BroadcastPacket(PacketType.Replication, packet);    
        }

        // Change Tree and send Update packet on U
        if (Input.GetKeyDown(KeyCode.U))
        {
            tree.name = "I UPDATED";
            if (server.replicator.context.TryGetNetworkID(tree, out int networkID))
            {
                var packet = new ReplicationPacket();
                packet.action = ReplicationAction.Update;
                packet.classID = NetworkObjectType.Tree;
                packet.networkID = networkID;
                packet.obj = tree;

                server.BroadcastPacket(PacketType.Replication, packet);
            }

        }

        // Delete Tree and send Destroy packet on D
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (server.replicator.context.TryGetNetworkID(tree, out int networkID))
            {
                var packet = new ReplicationPacket();
                packet.action = ReplicationAction.Destroy;
                packet.classID = NetworkObjectType.Tree;
                packet.networkID = networkID;
                packet.obj = tree; // Don't need to serialize object for Destroy

                server.BroadcastPacket(PacketType.Replication, packet);
            }
        }
    }

    private void OnDestroy()
    {
        ENet.Library.Deinitialize();
    }
}
