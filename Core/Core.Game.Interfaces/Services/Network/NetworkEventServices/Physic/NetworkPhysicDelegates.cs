using Core.Game.Models.Enum;
using Core.Game.Models.Vectors;
using Microsoft.Xna.Framework;

namespace Core.Game.Interfaces.Services.Network.NetworkEventServices.Physic;


public delegate void ServerUpdateBody(int index, EntityType type, Vector2 position, Vector2 velocity, 
    int rotation, bool includeSelf = false);