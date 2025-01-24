using BLL.Abstractions.Factories;
using Core.Entities;
using Core.Enums;

namespace BLL.Factories
{
    public abstract class ShipFactory
    {
        public abstract Ship CreateShip(string type, Point position, Direction direction);
    }

    public class DefaultShipFactory : IShipFactory
    {
        public Ship CreateShip(string type, Point position, Direction direction)
        {
            return type.ToLower() switch
            {
                "military" => new MilitaryShip(position, direction),
                "auxiliary" => new AuxiliaryShip(position, direction),
                "mixed" => new MixedShip(position, direction),
                _ => throw new ArgumentException($"Invalid ship type: {type}")
            };
        }
    }
}