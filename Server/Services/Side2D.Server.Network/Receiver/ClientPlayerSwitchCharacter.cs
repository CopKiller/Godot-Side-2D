using Core.Game.Models.Enum;
using Infrastructure.Network.Packet.Client;
using LiteNetLib;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor
{
    public void ClientPlayerSwitchCharacter(CPlayerSwitchCharacter obj, NetPeer netPeer)
    {
        players.TryGetValue(netPeer.Id, out var player);

        if (player == null) return;
            
        if (player.TempPlayer.ClientState != ClientState.Game) return;
            
        if (player.TempPlayer.CountCharacters() == 0) return;
            
        player.PlayerSwitchCharacter(netPeer.Id);
            
        ServerSendCharacters(netPeer);
    }
}