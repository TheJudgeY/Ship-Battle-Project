using Core.Entities;
using Core.Enums;

public class AuxiliaryShip : Ship
{
    public AuxiliaryShip(Point position, Direction direction)
        : base(speed: 3, length: 2, position, direction, attackRange: 0, healRange: 4)
    {
        Type = "Auxiliary";
    }
}