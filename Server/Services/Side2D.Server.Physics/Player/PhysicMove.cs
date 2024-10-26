using Core.Game.Interfaces.Physic.Player;
using Core.Game.Models;
using Core.Game.Models.Enum;
using Core.Game.Models.Player;
using Core.Game.Models.Vectors;
using Side2D.Server.Physics.Entity;

namespace Side2D.Server.Physics.Player;

public partial class PhysicPlayer
{
    private const double MovementMaxSpeed = 1.2; // 1.2 px per tick
    private long LastMovementTick { get; set; } = 0;

    public bool CanMove(Position newPosition, long currentTick)
    {
        // Verificar se o tick atual é válido
        if (currentTick <= LastMovementTick)
            return false; // Evita movimentos repetidos ou fora de ordem

        // Calcular o tempo decorrido em ticks
        var timeElapsed = currentTick - LastMovementTick;

        // Calcular a distância entre a posição atual e a nova posição
        var distance = newPosition.DistanceTo(playerModel.Position);

        // Calcular a distância máxima permitida com base no tempo decorrido
        var maxDistanceAllowed = timeElapsed * MovementMaxSpeed;

        // Verificar se a distância excede o permitido
        if (distance > maxDistanceAllowed)
        {
            // Movimento muito rápido
            return false;
        }

        // Atualizar o tick do último movimento e a posição do jogador
        LastMovementTick = currentTick;
        playerModel.Position.SetValues(newPosition);

        return true;
    }
}