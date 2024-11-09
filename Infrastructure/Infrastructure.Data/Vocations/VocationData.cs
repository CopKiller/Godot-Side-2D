using Core.Game.Interfaces.Data.Vocations;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Infrastructure.Data.Serializer;
using Infrastructure.Data.Vocations.Contract;

namespace Infrastructure.Data.Vocations;

public class VocationData(int maxValues, string currentDirectory) : BaseData<VocationContractData>(maxValues, currentDirectory)
{
    
    
}