using Godot;

namespace Side2D.scripts.Export;

public class MapExport
{
    public static void ExportMap(TileMapLayer[] tileMapLayer, string exportDirectory)
    {
        foreach (var tileMap in tileMapLayer)
        {
            var layerName = tileMap.Name;
            
            var rect = tileMap.GetUsedRect();
            
            var width = rect.Size.X;
            var height = rect.Size.Y;
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    
                    var vector = new Vector2I(x, y);

                    var cellSourceId = tileMap.GetCellSourceId(vector);
                    var tileLayerId = tileMap.TileSet.id
                    
                    var tile = tileMap.TileSet.collision
                    
                    data[x, y] = tile;
                }
            }
        }
    }
    
}