using Side2D.Models.Player;
using Side2D.Models.Vectors;

namespace Side2D.Models.Validation;

public static class EntityValidator
{
    public const int MaxCharacters = 3;

    public static Position DefaultPosition = new Position(new Vector2C(400, 300));
}