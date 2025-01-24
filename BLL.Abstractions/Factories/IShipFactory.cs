using Core.Entities;
using Core.Enums;

namespace BLL.Abstractions.Factories
{
    public interface IShipFactory
    {
        Ship CreateShip(string type, Point position, Direction direction);
    }
}