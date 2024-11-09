using Core.Game.Interfaces.Services.Network.NetworkEventServices.Attribute;
using Core.Game.Models.Enum;

namespace Side2D.Server.Attribute;

public class NetworkAttribute : INetworkAttribute
{
    public event ServerUpdateAttributes? OnServerUpdateAttributes;
    public event ServerUpdateVitals? OnServerUpdateVitals;
    public event ServerVitalsNotify? OnServerVitalsNotify;
    
    public void ServerUpdateAttributes(int playerIndex)
    {
        OnServerUpdateAttributes?.Invoke(playerIndex);
    }
    
    public void ServerUpdateVitals(int playerIndex)
    {
        OnServerUpdateVitals?.Invoke(playerIndex);
    }
    
    public void ServerVitalsNotify(int playerIndex, VitalType vitalType, double value)
    {
        OnServerVitalsNotify?.Invoke(playerIndex, vitalType, value);
    }
}