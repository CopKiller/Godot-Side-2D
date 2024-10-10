
using Core.Game.Models.Player;

namespace Side2D.Server.TempData.Temp.Interface;

public interface ITempUpdatePlayerVar : ITempData
{
    void SetPlayerCombat(bool inCombat);
    Action<Vitals> SendVitals { get; set; }
}