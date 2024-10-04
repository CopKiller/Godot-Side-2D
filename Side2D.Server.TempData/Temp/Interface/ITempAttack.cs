using Side2D.Server.TempData.Interface;

namespace Side2D.Server.TempData.Temp.Interface;

public interface ITempAttack : ITempData
{
    bool CanAttack(long currentTick);
}