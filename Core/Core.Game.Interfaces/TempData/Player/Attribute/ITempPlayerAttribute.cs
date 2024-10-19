namespace Core.Game.Interfaces.TempData.Player.Attribute;

public interface ITempPlayerAttribute : ITempData
{
    Action OnChange { get; set; }
}