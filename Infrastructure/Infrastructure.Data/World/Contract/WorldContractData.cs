using System.Runtime.Serialization;
using Core.Game.Interfaces.Data.Worlds;

namespace Infrastructure.Data.World.Contract;

[DataContract] public class WorldContractData : IWorldContractData
{
    [DataMember] public int Id { get; set; }
    [DataMember] public string WorldName { get; set; } = string.Empty;
    [DataMember] public int StartX { get; set; } = 100;
    [DataMember] public int StartY { get; set; } = 100;
    [DataMember] public int WorldWidth { get; set; } = 500;
    [DataMember] public int WorldHeight { get; set; } = 500;
    [DataMember] public List<ILayerContractData> LayerDataList { get; set; } = [];
    [IgnoreDataMember] private ILayerContractData[,]? LayerDataArray { get; set; }

    public void AddLayerData(LayerContractData data)
    {
        if (LayerDataList.FindIndex(x => x.X == data.X && x.Y == data.Y) == -1) LayerDataList.Add(data);
    }

    public void SyncListToArray() //--> Utilizado no servidor, para otimizar a busca de dados
    {
        if (LayerDataList.Count == 0)
            return;

        var maxX = LayerDataList.Max(x => x.X);
        var maxY = LayerDataList.Max(x => x.Y);

        LayerDataArray = new ILayerContractData[maxX + 1, maxY + 1];

        foreach (var data in LayerDataList) LayerDataArray[data.X, data.Y] = data;
    }
}