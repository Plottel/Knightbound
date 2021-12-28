using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft.Networking;
using System.IO;

public class NetworkManagerClient : DeftNetworkManagerClient
{
    public int playerObjNetworkID;


    private PlayerControllerClient controller;

    public override void OnAwake()
    {
        base.OnAwake();

        controller = new PlayerControllerClient();

        SetPacketHandler<WelcomePacketHandlerClient>(PacketType.Welcome);
        SetPacketHandler<ConsoleMessagePacketHandlerClient>(PacketType.ConsoleMessage);
        SetPacketHandler<ReplicationPacketHandlerClient>(PacketType.Replication);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // How to map Player ID to the Player Object network ID
        // Welcome packet needs to send that network ID i think.
        if (state == NetworkState.Welcomed)
            controller.OnUpdate();
    }

    public void JoinServer(string hostName, ushort port)
    {
        state = NetworkState.Connecting;
        client.JoinServer(hostName, port, OnServerConnectionEstablished);
    }

    // This is actually gameplay layer.
    // Begin thinking on how to factor out the NMC and NMS
    private void OnServerConnectionEstablished()
    {
        state = NetworkState.Connected;
        SendWelcomePacket(WelcomeState.PlayerID);
    }

    public void SetPlayerObjNetworkID(int playerObjNetworkID)
    {
        this.playerObjNetworkID = playerObjNetworkID;

        if (replicator.context.TryGetNetworkObject(playerObjNetworkID, out var obj))
            controller.player = obj as Tree;
    }


}
