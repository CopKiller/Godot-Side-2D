namespace Core.Game.Interfaces.Services.Network.NetworkEventServices.Attribute;

public interface INetworkAttribute
{
    event ServerUpdateAttributes OnServerUpdateAttributes;
    event ServerUpdateVitals OnServerUpdateVitals;
    
    void ServerUpdateAttributes(int playerIndex);
    void ServerUpdateVitals(int playerIndex);
}