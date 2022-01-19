using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deft;
using Deft.Networking;
using System.IO;

public class SetPlayerInfoPacketHandlerClient : PacketHandlerClient
{
    public override void HandlePacket(string originIP, BinaryReader reader)
    {
        int playerID = reader.ReadInt32();
        int characterID = reader.ReadInt32();

        PlayerManagerClient.Get.SetPlayerInfo(playerID, characterID);
    }
}
