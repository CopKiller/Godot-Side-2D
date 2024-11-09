using System.Runtime.Serialization;
using Core.Game.Interfaces.Data.Worlds;

namespace Infrastructure.Data.World.Contract;

[DataContract] public class LayerContractData : ILayerContractData
{
    [DataMember] public int X { get; set; }
    [DataMember] public int Y { get; set; }
    [DataMember] public bool IsCollision { get; set; }
}