
using Core.Game.Models.Vectors;

namespace Side2D.Server.TempData.Temp.Interface;

public interface ITempMove : ITempData
{
    bool CanMove(Vector2C position);
}