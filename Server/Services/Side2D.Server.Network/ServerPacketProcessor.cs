using Core.Game.Interfaces.Repositories;
using Core.Game.Models.Enum;
using Infrastructure.Logger;
using Infrastructure.Network;
using Infrastructure.Network.Packet.Client;
using LiteNetLib;
using Side2D.Server.Network.Interfaces;
using Side2D.Server.TempData.Interface;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor(IAccountRepository accountRepository,
    IPlayerRepository playerRepository,
    ITempDataService tempDataService,
    Dictionary<int, ServerClient> players)
    :PacketProcessor, IServerPacketProcessor
{
    
    public void Initialize()
    {
        this.RegisterCustomTypes();

        SubscribePacket();
    }

    public override void SubscribePacket()
    {
        // Register to receive packets  
        this.Subscribe<CPlayerLogin>(ClientLogin);
        this.Subscribe<CPlayerMove>(ClientPlayerMove);
        this.Subscribe<CAccountRegister>(ClientAccountRegister);
        this.Subscribe<CCreateCharacter>(ClientCreateCharacter);
        this.Subscribe<CPlayerUseCharacter>(ClientPlayerUseCharacter);
        this.Subscribe<CPlayerSwitchCharacter>(ClientPlayerSwitchCharacter);
        this.Subscribe<CPlayerAttack>(ClientPlayerAttack);
    }

    public void SendDataToAllBut<T>(NetPeer excludePeer, T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class, new()
    {
        var excludePeerId = excludePeer.Id;
            
        var allPlayers = players.Select(allPlayers => allPlayers.Value)
            .Where(player => player.Peer.Id != excludePeerId)
            .ToList();

        foreach (var player in allPlayers)
        {
            SendDataTo(player.Peer, packet, deliveryMethod);
        }
    }
        
    public void SendDataToAllBut<T>(NetPeer excludePeer, T packet, ClientState clientState, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class, new()
    {
        var excludePeerId = excludePeer.Id;
            
        var allPlayers = players.Select(allPlayers => allPlayers.Value)
            .Where(player => player.Peer.Id != excludePeerId && player.TempPlayer.ClientState == clientState)
            .ToList();

        foreach (var player in allPlayers)
        {
            SendDataTo(player.Peer, packet, deliveryMethod);
        }
    }

    public void SendDataToAll<T>(T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class, new()
    {
        foreach (var player in players)
        {
            SendDataTo(player.Value.Peer, packet, deliveryMethod);
        }
    }
        
    public void SendDataToAll<T>(T packet, ClientState clientState, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class, new()
    {
        foreach (var player in players
                     .Where(player => player.Value.TempPlayer.ClientState == clientState))
        {
            SendDataTo(player.Value.Peer, packet, deliveryMethod);
        }
    }

    public void SendDataTo<T>(NetPeer peer, T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class, new()
    {
        Log.PrintInfo(packet.GetType().ToString());
        base.Send(peer, packet, deliveryMethod);
    }
}
