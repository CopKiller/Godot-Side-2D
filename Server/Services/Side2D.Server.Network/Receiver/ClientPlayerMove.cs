using Core.Game.Models.Enum;
using Core.Game.Models.Vectors;
using Infrastructure.Network.Packet.Client;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor
{
    public void ClientPlayerMove(CPlayerMove obj, NetPeer netPeer)
    {
        players.TryGetValue(netPeer.Id, out var player);

        if (player == null) return;
            
        if (player.TempPlayer.ClientState != ClientState.Game) return;
        
        if ( !player.PhysicPlayer.PlayerMove(obj.Position))
        {
            return;
        }
    }
}