using Core.Entities;
using Core.Enums;

namespace BLL.Abstractions.Services
{
    public interface IFieldService
    {
        void AddShip(Ship ship, int fieldWidth, int fieldHeight);
        Ship GetShipAt(Point position);
        IEnumerable<Ship> GetAllShips();
        string? IsValidPlacement(int x, int y, Direction direction, int shipLength, int fieldWidth, int fieldHeight);
        Field GetField();
    }
}
