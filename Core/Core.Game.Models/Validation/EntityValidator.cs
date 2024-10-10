using Core.Game.Models.Player;
using Core.Game.Models.Vectors;

namespace Core.Game.Models.Validation;

public static class EntityValidator
{
    public const int MaxCharacters = 3;

    public static Position DefaultPosition = new Position(new Vector2C(400, 300));
}