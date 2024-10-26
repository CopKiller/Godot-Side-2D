
using Core.Game.Interfaces.Attribute;
using Core.Game.Interfaces.Combat;
using Core.Game.Interfaces.Physic;
using Core.Game.Interfaces.Physic.Player;
using Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;
using Core.Game.Models;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Side2D.Server.Physics.Player;

namespace Side2D.Server.Physics;

public class PhysicService(ICombatService combatService) : IPhysicService
{
    // TODO: Implement physics service
    // This service will be responsible for handling all physics related tasks
    
    public int DefaultUpdateInterval { get; set; } = 1;
    
    public INetworkPhysic NetworkEvents { get; } = new NetworkPhysic();
    
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
        foreach (var physicPlayer in PhysicPlayers.Values)
        {
            physicPlayer?.Update(currentTick);
        }
    }
    
    public void Dispose()
    {
        PhysicEntities.ForEach(entity => entity.Dispose());
        PhysicEntities.Clear();
        PhysicPlayers.Clear();
    }
    
    public void AddPlayerPhysic(int index, PlayerModel playerModel)
    {
        var physicPlayer = new PhysicPlayer(index, playerModel, NetworkEvents, this);
        
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
    
    public List<IPhysicPlayer?> GetPlayersInRadius(Position position, int range)
    {
        return PhysicPlayers.Values.Where(player => player?.DistanceTo(position) <= range).ToList();
    }
    
    public void NotifyCombatService(int attackerIndex, int targetIndex)
    {
        combatService.NotifyReceivePlayerAttack(attackerIndex, targetIndex);
    }
}