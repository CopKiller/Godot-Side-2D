namespace Core.Game.Interfaces.Data.Worlds;

public interface IWorldContractData : IContractData
{
    string WorldName { get; set; }
    int StartX { get; set; }
    int StartY { get; set; }
    int WorldWidth { get; set; }
    int WorldHeight { get; set; }
    List<ILayerContractData> LayerDataList { get; set; }
}