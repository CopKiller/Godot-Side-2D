using Core.Game.Models.Enum;
using Infrastructure.Data.Vocations;
using Infrastructure.Data.World;

namespace Infrastructure.Data;

public class GameData
{
    private const string DataPath = "/Data";
    private const string DataExtension = ".xml";

    private WorldData _worldData;
    private VocationData _vocationData;
    
    public GameData()
    {
        var currentDirectory = Environment.CurrentDirectory + DataPath;
        
        if (!Directory.Exists(currentDirectory))
        {
            Directory.CreateDirectory(currentDirectory);
        }

        var path = currentDirectory + "/World/~/" + DataExtension;
        _worldData = new WorldData(2, path);

        path = currentDirectory + "/Vocation/~/" + DataExtension;
        _vocationData = new VocationData((byte)Vocation.Count - 1, path);
        
        LoadGameData();
    }

    private void LoadGameData()
    {
        _worldData.Load();
        _vocationData.Load();
    }
}