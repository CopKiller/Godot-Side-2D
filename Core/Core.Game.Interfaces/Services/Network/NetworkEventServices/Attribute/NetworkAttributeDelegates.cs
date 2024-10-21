using Core.Game.Models.Player;

namespace Core.Game.Interfaces.Services.Network.NetworkEventServices.Attribute;



public delegate void ServerUpdateAttributes(int playerIndex);
public delegate void ServerUpdateVitals(int playerIndex);