using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Deft.Networking
{
    public class DeftNetworkManagerClient : Manager<DeftNetworkManagerClient>
    {
        public NetworkState state;
        public int playerID;
        public NetworkReplicator replicator;

        protected NetworkClient client;
        
        private Dictionary<PacketType, PacketHandlerClient> packetHandlers;

        public override void OnAwake()
        {
            base.OnAwake();

            replicator = new NetworkReplicator();
            client = new NetworkClient();
            packetHandlers = new Dictionary<PacketType, PacketHandlerClient>();
        }

        public override void OnUpdate()
        {
            ProcessIncomingPackets();
        }

        public void SetContext(NetworkContext context)
        {
            replicator.context = context;
        }

        public void SetPlayerID(int playerID)
        {
            this.playerID = playerID;
        }

        public void SetPacketHandler<T>(PacketType packetType) where T : PacketHandlerClient
        {
            T packetHandler = Activator.CreateInstance<T>();
            packetHandlers[packetType] = packetHandler;
        }

        public void ProcessIncomingPackets()
        {
            if (state == NetworkState.Uninitialized)
                return;

            using (MemoryStream stream = new MemoryStream())
            {
                while (client.PumpPacket(stream))
                {
                    stream.Position = 0;

                    // Memory Stream containing PacketData
                    using (BinaryReader reader = new BinaryReader(stream, Encoding.Default, true))
                    {
                        string ip = reader.ReadString();
                        PacketType packetType = (PacketType)reader.ReadInt32();

                        HandlePacket(ip, packetType, reader);
                    }
                }
            }
        }

        private void HandlePacket(string ip, PacketType packetType, BinaryReader reader)
        {
            Debug.Log("CLIENT: Packet Received. Type: " + packetType.ToString());
            packetHandlers[packetType].HandlePacket(ip, reader);
        }

        public void SendPacket(MemoryStream stream)
        {
            client.SendPacket(stream);
        }

        public void SendInputPacket(InputState inputState)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
                {
                    writer.Write((int)PacketType.Input);
                    writer.Write(playerID);
                    inputState.Serialize(writer);
                }

                client.SendPacket(stream);
            }
        }

        public void SendConsoleMessagePacket(string message)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, true))
                {
                    writer.Write((int)PacketType.ConsoleMessage);
                    writer.Write(message);
                }

                client.SendPacket(stream);
            }
        }
    }
}
