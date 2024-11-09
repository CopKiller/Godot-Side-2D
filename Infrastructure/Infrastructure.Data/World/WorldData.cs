using System.Xml.Serialization;
using Infrastructure.Data.World.Contract;
using XmlSerializer = Infrastructure.Data.Serializer.XmlSerializer;

namespace Infrastructure.Data.World;

public class WorldData(int maxValues, string currentDirectory) : BaseData<WorldContractData>(maxValues, currentDirectory)
{
    
}