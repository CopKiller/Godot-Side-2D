using Core.Game.Interfaces.Data;
using Infrastructure.Data.Serializer;
using Infrastructure.Data.Vocations.Contract;

namespace Infrastructure.Data;

public class BaseData<T> where T : class, IContractData, new()
{
    private int MaxValues;
    private readonly string _currentDirectory;

    public List<T> DataObject = [];

    protected BaseData(int maxValues, string dataPath)
    {
        MaxValues = maxValues;
        _currentDirectory = $"{dataPath}";
    }

    public void Load()
    {
        for (var i = 1; i <= MaxValues; i++)
        {
            var path = _currentDirectory.ToIndex(i);
            CheckArchive(i, path);
            DataObject.Add(XmlSerializer.DeserializeObjectFromPath<T>(path));
        }
    }
    
    private void CheckArchive(int id, string path)
    {
        if (!File.Exists(path))
            CreateArchive(id, path);
    }
    
    private void CreateArchive(int id, string path)
    {
        var obj = new T
        {
            Id = id
        };

        XmlSerializer.SerializeObject(path, obj);
    }
}