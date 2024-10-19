using Core.Game.Interfaces.Combat.Player;
using Core.Game.Models;

namespace Side2D.Server.Combat.Player;

public class CombatPlayer : CombatEntity, ICombatPlayer
{
    public int Index { get; }
    private PlayerModel PlayerModel { get; }
    
    public CombatPlayer(int index, PlayerModel playerModel)
    {
        Index = index;
        PlayerModel = playerModel;
    }

}