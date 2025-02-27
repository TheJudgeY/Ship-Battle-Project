using Core.Entities;
using Core.Enums;
using Core.Utilities;

namespace BLL.Abstractions.Services
{
    public interface IFieldService
    {
        OperationResult<bool> AddShip(Ship ship, int fieldWidth, int fieldHeight);
        OperationResult<Ship> GetShipAt(Point position);
        OperationResult<IEnumerable<Ship>> GetAllShips();
        OperationResult<bool> IsValidPlacement(int x, int y, Direction direction, int shipLength, int fieldWidth, int fieldHeight);
        OperationResult<bool> RemoveShip(Point position);
        Field GetField();
        OperationResult<bool> SaveChanges();
    }
}
