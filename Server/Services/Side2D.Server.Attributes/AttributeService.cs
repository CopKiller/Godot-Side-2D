﻿using Core.Game.Interfaces.Attribute;
using Core.Game.Interfaces.Attribute.Player;
using Core.Game.Interfaces.Combat;
using Core.Game.Interfaces.Services.Network.NetworkEventServices.Attribute;
using Core.Game.Models;
using Side2D.Server.Attribute.Player;

namespace Side2D.Server.Attribute;

public class AttributeService() : IAttributeService
{
    // TODO: Implement Attribute service
    // This service will be responsible for handling all Attribute related tasks
    
    public int DefaultUpdateInterval { get; set; } = 1;

    public INetworkAttribute NetworkEvents { get; } = new NetworkAttribute();
    
    private List<IAttributeEntity> AttributeEntities { get; } = [];
    
    private Dictionary<int, IAttributePlayer?> AttributePlayers { get; } = new();
    
    public void ReceiveCombatDamage(int attackerIndex, int victimIndex)
    {
        var victim = AttributePlayers.GetValueOrDefault(victimIndex);
        if (victim == null) return;
        var attacker = AttributePlayers.GetValueOrDefault(attackerIndex);
        if (attacker == null) return;
        
        // Get Damage Of Attacker
        var damage = attacker.GetDamage();
        
        victim?.TakeDamage(damage);
    }
    
    public void SetPlayerCombatState(int index, bool inCombat)
    {
        var player = AttributePlayers.GetValueOrDefault(index);
        if (player == null) return;
        
        player.SetCombatState(inCombat);
    }
    

    public void Register()
    {
        
    }

    public void Start()
    {
        
    }

    public void Stop()
    {
        Dispose();
    }

    public void Restart()
    {
        Dispose();
        Start();
    }

    public void Update(long currentTick)
    {
        foreach (var entity in AttributeEntities)
        {
            entity.Update(currentTick);
        }
    }
    
    public void Dispose()
    {
        AttributeEntities.ForEach(entity => entity.Dispose());
        AttributeEntities.Clear();
        AttributePlayers.Clear();
    }
    
    public void AddPlayerAttribute(int index, PlayerModel playerModel)
    {
        var physicPlayer = new AttributePlayer(index, playerModel.Attributes, playerModel.Vitals, NetworkEvents);
        
        AttributeEntities.Add(physicPlayer);
        AttributePlayers.Add(index, physicPlayer);
    }

    public void RemovePlayerAttribute(int index)
    {
        var physicPlayer = AttributePlayers.GetValueOrDefault(index);
        if (physicPlayer == null) return;
        AttributeEntities.Remove(physicPlayer);
        AttributePlayers.Remove(index);
    }

    public IAttributePlayer? GetPlayerAttribute(int index)
    {
        return AttributePlayers.GetValueOrDefault(index);
    }
}