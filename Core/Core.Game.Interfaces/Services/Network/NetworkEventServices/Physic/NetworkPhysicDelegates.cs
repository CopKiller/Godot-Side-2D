using Core.Game.Models.Enum;
using Core.Game.Models.Vectors;

namespace Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;



public delegate void ServerUpdatePosition(int playerIndex, bool includeSelf);
public delegate void ServerPlayerImpact(int playerIndex, Vector2C impactVelocity);
public delegate void ServerPlayerAttack(int playerIndex, AttackType attackType);