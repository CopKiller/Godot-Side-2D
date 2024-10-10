namespace Side2D.scripts.Network;

public interface IPacketHandler
{
    void RegisterPacketHandlers();
    
    void _ExitTree();
}