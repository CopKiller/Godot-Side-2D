using Side2D.Server.TempData.Interface;
using Side2D.Server.TempData.Temp.Player;

namespace Side2D.Server.TempData.Temp.Interface;

public interface ITempUpdatePlayerVar : ITempData
{
    void SetPlayerCombat(bool inCombat);
    Action? SendVitals { get; set; }
}