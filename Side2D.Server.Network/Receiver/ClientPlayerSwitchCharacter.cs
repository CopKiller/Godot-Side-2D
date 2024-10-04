﻿
using LiteNetLib;
using Side2D.Logger;
using Side2D.Models;
using Side2D.Models.Enum;
using Side2D.Models.Validation;
using Side2D.Network.CustomDataSerializable;
using Side2D.Network.Packet.Client;
using Side2D.Network.Packet.Server;

namespace Side2D.Server.Network
{
    public partial class ServerPacketProcessor
    {
        public void ClientPlayerSwitchCharacter(CPlayerSwitchCharacter obj, NetPeer netPeer)
        {
            if (ServerNetworkService.Players == null) return;
            
            ServerNetworkService.Players.TryGetValue(netPeer.Id, out var player);

            if (player == null) return;
            
            if (player.ClientState != ClientState.Game) return;
            
            if (player.PlayerModels.Count == 0) return;
            
            // Clear the player data model
            player.PlayerDataModel = new PlayerDataModel();
            // Clear the player move model
            player.PlayerMoveModel = new PlayerMoveModel();
            
            player.PlayerSwitchCharacter(netPeer.Id);
            
            ServerSendCharacters(netPeer);
        }
    }
}
