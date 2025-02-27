using Core.Entities;
using Core.Enums;

public class MilitaryShip : Ship
{
    public MilitaryShip(Point position, Direction direction)
        : base(speed: 5, length: 3, position, direction, attackRange: 5, healRange: 0)
    {
        Type = "Military";
    }
}