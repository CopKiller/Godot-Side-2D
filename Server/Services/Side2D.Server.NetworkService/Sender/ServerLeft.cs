using Core.Game.Models.Enum;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.NetworkService;

public partial class ServerPacketProcessor
{
    // TODO: Falta jogar o pacote para dentro do servidor de dados temporários do player...
    
    // TODO: Provavelmente vou remover isso aqui, e deixar apenas o SClientState responsável por isso...
    public void ServerLeft(NetPeer netPeer, SPlayerLeft sPlayerLeft, bool isDisconnect = true)
    {
        players.TryGetValue(netPeer.Id, out var player);

        if (player == null) return;
            
        SendDataToAllBut(netPeer, sPlayerLeft, ClientState.Game, DeliveryMethod.ReliableUnordered);

        if (!isDisconnect) return;
                
        player.Peer.Disconnect();
    }
}