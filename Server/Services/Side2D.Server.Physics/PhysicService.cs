
using Core.Game.Interfaces.Attribute;
using Core.Game.Interfaces.Combat;
using Core.Game.Interfaces.Physic;
using Core.Game.Interfaces.Physic.Player;
using Core.Game.Models;
using Core.Game.Models.Player;
using Side2D.Server.Physics.Player;

namespace Side2D.Server.Physics;

public class PhysicService(ICombatService combatService) : IPhysicService
{
    // TODO: Implement physics service
    // This service will be responsible for handling all physics related tasks
    
    public int DefaultUpdateInterval { get; set; } = 1;
    
    private List<IPhysicEntity> PhysicEntities { get; } = [];
    private Dictionary<int, IPhysicPlayer?> PhysicPlayers { get; } = new();

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
        PhysicEntities.ForEach(entity => entity.Update(currentTick));
    }
    
    public void Dispose()
    {
        PhysicEntities.ForEach(entity => entity.Dispose());
        PhysicEntities.Clear();
        PhysicPlayers.Clear();
    }
    
    public void AddPlayerPhysic(int index, PlayerModel playerModel)
    {
        var physicPlayer = new PhysicPlayer(index, playerModel, this);
        
        PhysicEntities.Add(physicPlayer);
        PhysicPlayers.Add(index, physicPlayer);
    }

    public void RemovePlayerPhysic(int index)
    {
        var physicPlayer = PhysicPlayers.GetValueOrDefault(index);
        if (physicPlayer == null) return;
        PhysicEntities.Remove(physicPlayer);
        PhysicPlayers.Remove(index);
    }

    public IPhysicPlayer? GetPlayerPhysic(int index)
    {
        return PhysicPlayers.GetValueOrDefault(index);
    }
    
    public void CheckPlayerAttack(int index, int range)
    {
        var physicPlayer = PhysicPlayers.GetValueOrDefault(index);
        
        if (physicPlayer == null) return;
        
        var position = physicPlayer.GetPosition();
        // Buscar jogadores no range de ataque
        var playersInRadius = GetPlayersInRadius(position, range);
        
        foreach (var player in playersInRadius)
        {
            if (player == null) continue;
            if (player.Index == index) continue;
            
            combatService.ReceivePlayerAttack(index, player.Index);
        }
    }
    
    private List<IPhysicPlayer?> GetPlayersInRadius(Position position, int radius)
    {
        return PhysicPlayers.Values.Where(player => player?.DistanceTo(position) <= radius).ToList();
    }
}