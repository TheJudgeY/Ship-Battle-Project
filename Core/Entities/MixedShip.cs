using Core.Entities;
using Core.Enums;

public class MixedShip : Ship
{
    public MixedShip(Point position, Direction direction)
        : base(speed: 4, length: 4, position, direction, attackRange: 3, healRange: 2)
    {
        Type = "Mixed";
    }
}