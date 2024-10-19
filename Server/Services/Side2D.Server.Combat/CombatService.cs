using Core.Game.Interfaces.Attribute;
using Core.Game.Interfaces.Combat;
using Core.Game.Interfaces.Combat.Player;
using Core.Game.Models;
using Side2D.Server.Combat.Player;

namespace Side2D.Server.Combat;

public class CombatService(IAttributeService attributeService) : ICombatService, ICombatStatusProvider
{
    private List<ICombatEntity> _entities = [];
    
    private Dictionary<int, ICombatPlayer> _players = new();
    
    private int _defaultUpdateInterval = 1;
    
    public void ReceivePlayerAttack(int attackerIndex, int victimIndex)
    {
        Console.WriteLine($"Player {attackerIndex} attacked player {victimIndex}");
        
        foreach (var player in _players)
        {
            if (player.Key == victimIndex || player.Key == attackerIndex)
                player.Value.SetCombatState(true);

            if (player.Key == victimIndex)
            {
                attributeService.ReceiveCombatDamage(attackerIndex, victimIndex);
            }
        }
    }
    public void AddPlayerCombat(int index, PlayerModel playerModel)
    {
        var combatPlayer = new CombatPlayer(index, playerModel);
        _entities.Add(combatPlayer);
        _players.Add(index, combatPlayer);
    }
    
    public void RemovePlayerCombat(int index)
    {
        if (!_players.TryGetValue(index, out var combatPlayer)) return;

        _entities.Remove(combatPlayer);
        _players.Remove(index);
    }
    
    public void Register()
    {
        
    }

    public void Start()
    {
        
    }

    public void Stop()
    {
        
    }

    public void Restart()
    {
        
    }

    public void Update(long currentTick)
    {
        _entities.ForEach(a => a.Update(currentTick));
    }
    
    public void Dispose()
    {
        _entities.ForEach(a => a.Dispose());
    }

    public bool GetCombatState(int index)
    {
        var player = _players.GetValueOrDefault(index);
        return player?.GetCombatState() ?? false;
    }
}