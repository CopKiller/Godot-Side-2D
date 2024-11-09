using System.Runtime.Serialization;
using Infrastructure.Data.World.Contract;

namespace Infrastructure.Data.Serializer;

public class XmlSerializer
{
    public static bool SerializeObject<T>(string path, T obj)
    {
        var serializer = new DataContractSerializer(typeof(T));

        // Usa FileMode.Create para criar um novo arquivo ou sobrescrever se já existir
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        serializer.WriteObject(stream, obj);
        return true;
    }

    public static T DeserializeObjectFromPath<T>(string path)
    {
        var serializer = new DataContractSerializer(typeof(T));

        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        var obj = (T)serializer.ReadObject(stream)!;
        if (obj is WorldContractData mapLayerData)
        {
            mapLayerData.SyncListToArray();
        }
        return obj;
    }
}