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
    [DataMember] public List<LayerContractData> layerDataList { get; set; } = [];
    [IgnoreDataMember] public LayerContractData[,] layerDataArray { get; set; }

    public void AddLayerData(LayerContractData data)
    {
        if (layerDataList.FindIndex(x => x.X == data.X && x.Y == data.Y) == -1) layerDataList.Add(data);
    }

    public void SyncListToArray() //--> Utilizado no servidor, para otimizar a busca de dados
    {
        if (layerDataList.Count == 0)
            return;

        var maxX = layerDataList.Max(x => x.X);
        var maxY = layerDataList.Max(x => x.Y);

        layerDataArray = new LayerContractData[maxX + 1, maxY + 1];

        foreach (var data in layerDataList) layerDataArray[data.X, data.Y] = data;
    }
}