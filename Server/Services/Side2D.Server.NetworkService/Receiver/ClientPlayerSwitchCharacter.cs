using Core.Game.Models.Enum;
using Infrastructure.Network.Packet.Client;
using LiteNetLib;

namespace Side2D.Server.NetworkService;

public partial class ServerPacketProcessor
{
    public void ClientPlayerSwitchCharacter(CPlayerSwitchCharacter obj, NetPeer netPeer)
    {
        players.TryGetValue(netPeer.Id, out var player);

        if (player == null) return;
            
        if (player.TempPlayer.ClientState != ClientState.Game) return;
            
        if (player.TempPlayer.CountCharacters() == 0) return;
            
        player.PlayerSwitchCharacter(netPeer.Id);
        physicService.RemovePhysicEntity(1, player.Index, EntityType.Player);
        attributeService.RemovePlayerAttribute(player.Index);
        combatService.RemovePlayerCombat(player.Index);
        
        ServerSendCharacters(netPeer);
    }
}