using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor
{
    public void ServerPlayerImpact(int index, Vector2C impactVelocity)
    {
        players.TryGetValue(index, out var player);
            
        if (player == null) return;
        
        if (player.TempPlayer.ClientState != ClientState.Game) return;
        
        var packet = SPlayerImpact.Create(index, impactVelocity);
            
        SendDataToAll(packet, ClientState.Game, DeliveryMethod.ReliableUnordered);
    }
}