using Core.Entities;

namespace BLL.Abstractions.Services
{
    public interface IFieldService
    {
        void AddShip(Ship ship, int fieldWidth, int fieldHeight);
        Ship GetShipAt(Point position);
        IEnumerable<Ship> GetAllShips();
    }
}
