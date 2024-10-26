using Core.Game.Interfaces.Attribute;
using Core.Game.Interfaces.Combat;
using Core.Game.Interfaces.Physic;
using Core.Game.Interfaces.Repositories;
using Core.Game.Interfaces.Services.Network.NetworkEventServices.Attribute;
using Core.Game.Interfaces.TempData;
using Core.Game.Models.Enum;
using Infrastructure.Logger;
using Infrastructure.Network;
using Infrastructure.Network.Packet.Client;
using LiteNetLib;
using Side2D.Server.Network.Interfaces;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor(
    IAccountRepository accountRepository,
    IPlayerRepository playerRepository,
    ITempDataService tempDataService,
    IPhysicService physicService,
    IAttributeService attributeService,
    ICombatService combatService,
    Dictionary<int, ServerClient> players)
    : PacketProcessor, IServerPacketProcessor
{

    public void Initialize()
    {
        RegisterCustomTypes();

        SubscribePacket();
        
        AssignNetworkSendersOnServices();
    }

    public void SubscribePacket()
    {
        // Register to receive packets  
        Subscribes<CPlayerLogin>(ClientLogin);
        Subscribes<CPlayerMove>(ClientPlayerMove);
        Subscribes<CAccountRegister>(ClientAccountRegister);
        Subscribes<CCreateCharacter>(ClientCreateCharacter);
        Subscribes<CPlayerUseCharacter>(ClientPlayerUseCharacter);
        Subscribes<CPlayerSwitchCharacter>(ClientPlayerSwitchCharacter);
        Subscribes<CPlayerAttack>(ClientPlayerAttack);

        return;
        
        void Subscribes<T>(Action<T, NetPeer> onReceive) where T : class, new()
        {
            SubscribeReusable(onReceive);

            //Log.PrintError(onReceive.Method.Username + " Subscribed");
        }
    }
    
    private void AssignNetworkSendersOnServices()
    {
        //attributeService.NetworkEvents.ServerUpdateAttributes += ServerUpdateAttributes;
        attributeService.NetworkEvents.OnServerUpdateVitals += ServerUpdateVitals;
        physicService.NetworkEvents.OnServerUpdatePosition += ServerUpdatePosition;
        physicService.NetworkEvents.OnServerUpdateKnockback += ServerUpdateKnockback;
        tempDataService.NetworkEvents.OnDbSavePlayer += playerRepository.UpdatePlayerAsync;
        tempDataService.NetworkEvents.OnServerClientState += ServerClientState;
        physicService.NetworkEvents.OnServerUpdateAttack += ServerUpdateAttack;
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
