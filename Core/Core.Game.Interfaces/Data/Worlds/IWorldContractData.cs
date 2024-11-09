namespace Core.Game.Interfaces.Data.Worlds;

public interface IWorldContractData : IContractData
{
    string WorldName { get; set; }
    int StartX { get; set; }
    int StartY { get; set; }
}