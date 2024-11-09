using Core.Game.Models.Enum;

namespace Core.Game.Interfaces.Services.Network.NetworkEventServices.Attribute;

public interface INetworkAttribute
{
    event ServerUpdateAttributes OnServerUpdateAttributes;
    event ServerUpdateVitals OnServerUpdateVitals;
    event ServerVitalsNotify OnServerVitalsNotify;
    
    void ServerUpdateAttributes(int playerIndex);
    void ServerUpdateVitals(int playerIndex);
    void ServerVitalsNotify(int playerIndex, VitalType vitalType, double value);
}