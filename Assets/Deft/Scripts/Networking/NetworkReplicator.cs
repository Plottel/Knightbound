using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Deft.Networking
{
    public class NetworkReplicator
    {
        public NetworkContext context;

        public NetworkReplicator()
        {
            context = new NetworkContext();
        }

        public NetworkObject[] GetNetworkObjects()
        {
            return context.GetNetworkObjects();
        }

        public void ProcessReplicationCommand(ReplicationCommand command)
        {
            ReplicationAction action = command.action;

            switch (command.action)
            {
                case ReplicationAction.Create: ProcessCreate(command); break;
                case ReplicationAction.Update: ProcessUpdate(command); break;
                case ReplicationAction.Destroy: ProcessDestroy(command); break;
            }
        }

        // Create the object if it doesn't exist, and update it.
        private void ProcessCreate(ReplicationCommand command)
        {
            Debug.Log("Receiving Create - ID " + command.networkID.ToString());

            NetworkObject obj;
            if (!context.TryGetNetworkObject(command.networkID, out obj))
            {
                obj = NetworkPrefabRegistry.Create(command.classID);
                context.RegisterNetworkObject(command.networkID, obj);
            }

            obj.Deserialize(command.reader); // Deserialize object.
        }

        // Try to get the object and update it if it exists
        private void ProcessUpdate(ReplicationCommand command)
        {
            Debug.Log("Receiving Update - ID " + command.networkID.ToString());

            NetworkObject obj;
            if (!context.TryGetNetworkObject(command.networkID, out obj))
            {
                obj = NetworkPrefabRegistry.Create(command.classID); // DON'T register dummy.
                obj.Deserialize(command.reader); // Advance stream regardless of if we have the object.
                Object.Destroy(obj.gameObject);
            }
            else // Object already exists
            {
                obj.Deserialize(command.reader);
            }
        }

        // Try to get the object and destroy it if it exists
        private void ProcessDestroy(ReplicationCommand command)
        {
            if (context.TryGetNetworkObject(command.networkID, out NetworkObject obj))
            {
                context.DeregisterNetworkObject(command.networkID);
                Object.Destroy(obj.gameObject);
            }
        }
    }
}
