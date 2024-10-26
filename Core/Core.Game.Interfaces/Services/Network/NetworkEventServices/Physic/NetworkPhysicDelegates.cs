using Core.Game.Models.Enum;
using Core.Game.Models.Vectors;

namespace Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;



public delegate void ServerUpdatePosition(int index, EntityType type, bool includeSelf);
public delegate void ServerUpdateKnockback(int index, EntityType type, bool includeSelf, VectorTwo impactVelocity);
public delegate void ServerUpdateAttack(int index, EntityType type, bool includeSelf, AttackType attackType);