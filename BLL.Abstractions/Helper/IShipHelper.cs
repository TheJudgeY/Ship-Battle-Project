using Core.Entities;

namespace BLL.Abstractions.Helpers
{
    public interface IShipHelper
    {
        IEnumerable<Point> GetOccupiedPoints(Ship ship);
    }
}