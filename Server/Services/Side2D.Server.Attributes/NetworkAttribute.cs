using Core.Game.Interfaces.Services.Network.NetworkEventServices.Attribute;

namespace Side2D.Server.Attribute;

public class NetworkAttribute : INetworkAttribute
{
    public event ServerUpdateAttributes? OnServerUpdateAttributes;
    public event ServerUpdateVitals? OnServerUpdateVitals;
    
    public void ServerUpdateAttributes(int playerIndex)
    {
        OnServerUpdateAttributes?.Invoke(playerIndex);
    }
    
    public void ServerUpdateVitals(int playerIndex)
    {
        OnServerUpdateVitals?.Invoke(playerIndex);
    }
}