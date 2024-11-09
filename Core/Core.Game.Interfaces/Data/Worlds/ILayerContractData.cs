namespace Core.Game.Interfaces.Data.Worlds;

public interface ILayerContractData
{
    int X { get; set; }
    int Y { get; set; }
    bool IsCollision { get; set; }
}