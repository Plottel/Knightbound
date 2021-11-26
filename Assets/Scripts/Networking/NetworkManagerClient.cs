//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class NetworkManagerClient
//{
//    private GameClient client;

//    public NetworkManagerClient()
//    {
//        client = new GameClient();
//        client.SetPacketHandler<WelcomePacketHandlerClient>(PacketType.Welcome);
//    }

//    public void JoinServer(string hostName, ushort port)
//    {
//        client.JoinServer(hostName, port);
//    }

//    public void SendPacket(PacketType packetType, GamePacket packet)
//    {
//        client.SendPacket(packetType, packet);
//    }

//    public void Update()
//    {
//        // Receive packets as often as possible
//        client.ProcessPackets();
//    }
//}
