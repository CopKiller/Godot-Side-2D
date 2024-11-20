using Core.Game.Interfaces.Attribute.Player;
using Core.Game.Interfaces.Combat.Player;
using Core.Game.Models;
using Core.Game.Models.Enum;

namespace Side2D.Server.CombatService.Player;

public class CombatPlayer : CombatEntity, ICombatPlayer
{
    public override EntityType Type { get; } = EntityType.Player;
    public int Index { get; }
    private PlayerModel PlayerModel { get; }
    
    public CombatPlayer(int index, PlayerModel playerModel)
    {
        Index = index;
        PlayerModel = playerModel;
    }
}