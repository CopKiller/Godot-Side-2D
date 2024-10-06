using Side2D.Models.Vectors;
using Side2D.Server.TempData.Interface;

namespace Side2D.Server.TempData.Temp.Interface;

public interface ITempMove : ITempData
{
    bool CanMove(Vector2C position);
}