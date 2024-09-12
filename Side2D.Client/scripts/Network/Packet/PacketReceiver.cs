using Side2D.Logger;
using Side2D.Network.Packet.Server;

namespace Side2D.scripts.Network.Packet;

public class PacketReceiver
{
    public void Initialize()
    {
        ClientPacketProcessor.Instance.SubscribeReusable<SPlayerData>(ServerPlayerData);
        ClientPacketProcessor.Instance.SubscribeReusable<SPlayerMove>(ServerPlayerMove);
        ClientPacketProcessor.Instance.SubscribeReusable<SPlayerLeft>(ServerLeft);
    }
    
    private void ServerPlayerData(SPlayerData obj)
    {
        Map.Instance.AddPlayers(obj);
    }
    
    private void ServerPlayerMove(SPlayerMove obj)
    {
        Map.Instance._players.PlayerMove(obj.PlayerMoveModel);
    }
    
    private void ServerLeft(SPlayerLeft obj)
    {
        Map.Instance._players.RemovePlayer(obj.Index);
    }
    
}