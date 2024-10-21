using Core.Game.Models.Enum;
using Infrastructure.Network.CustomDataSerializable;
using Infrastructure.Network.Packet.Client;
using Infrastructure.Network.Packet.Server;
using LiteNetLib;

namespace Side2D.Server.Network;

public partial class ServerPacketProcessor
{
    public void ClientPlayerUseCharacter(CPlayerUseCharacter obj, NetPeer netPeer)
    {
        players.TryGetValue(netPeer.Id, out var player);

        if (player.TempPlayer.ClientState != ClientState.Character) return;

        if (player.TempPlayer.CountCharacters() == 0)
        {
            ServerAlert(netPeer, "You don't have any characters!");
            return;
        }

        // Buscar o PlayerDataModel diretamente
        var playerModel = player.TempPlayer.GetCharacter(obj.SlotNumber);
            
        if (playerModel == null)
        {
            ServerAlert(netPeer, "Character not found!");
            return;
        }
        
        // Atualiza o estado do jogador
        player.TempPlayer.ChangeState(ClientState.Game, obj.SlotNumber);
            
        player.PlayerDataModel = new PlayerDataModel(netPeer.Id, playerModel);
        player.PlayerMoveModel = new PlayerMoveModel(netPeer.Id, playerModel);
        
        // Services
        physicService.AddPlayerPhysic(player.Index, playerModel);
        attributeService.AddPlayerAttribute(player.Index, playerModel);
        combatService.AddPlayerCombat(player.Index, playerModel);
        
        // Get
        var playerPhysic = physicService.GetPlayerPhysic(player.Index);
        var playerAttribute = attributeService.GetPlayerAttribute(player.Index);
        //var playerCombat = combatService.GetPlayerCombat...
        player.AddPlayerServices(playerPhysic, playerAttribute);
        
        
        // TODO: Preciso desacoplar a necessidade de usar um PlayerDataModel... deixar pro servidor de dados temporários tratar deste envio.
        // Cria o pacote com os dados do jogador
        var packet = new SPlayerData();
        packet.PlayersDataModels.Add(player.PlayerDataModel);
        packet.PlayersMoveModels.Add(player.PlayerMoveModel);
        // Envia os dados do jogador para todos os outros jogadores
        SendDataToAllBut(netPeer, packet, ClientState.Game, DeliveryMethod.ReliableOrdered);
        
        
        // Enviar todos os jogadores incluindo o novo jogador para ele.
        var otherPlayers = players.Values
            .Where(x => x.TempPlayer.ClientState == ClientState.Game && x.Index != player.Index);
        var serverClients = otherPlayers as ServerClient[] ?? otherPlayers.ToArray();
        packet.PlayersDataModels.AddRange(serverClients.Select(x => x.PlayerDataModel));
        packet.PlayersMoveModels.AddRange(serverClients.Select(x => x.PlayerMoveModel));
        SendDataTo(netPeer, packet, DeliveryMethod.ReliableOrdered);
        
        // Envia uma atualização de vitals, pra carregar na game bars
        ServerUpdateVitals(player.Index);
    }
}