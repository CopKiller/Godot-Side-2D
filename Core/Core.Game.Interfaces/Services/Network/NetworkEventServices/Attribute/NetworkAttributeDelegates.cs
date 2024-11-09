using Core.Game.Models.Enum;
using Core.Game.Models.Player;

namespace Core.Game.Interfaces.Services.Network.NetworkEventServices.Attribute;



public delegate void ServerUpdateAttributes(int playerIndex);
public delegate void ServerUpdateVitals(int playerIndex);
public delegate void ServerVitalsNotify(int playerIndex, VitalType vitalType, double value);