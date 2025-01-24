using BLL.Abstractions.Helpers;
using Core.Entities;

namespace BLL.Helper
{
    public class ShipHelper : IShipHelper
    {
        public IEnumerable<Point> GetOccupiedPoints(Ship ship)
        {
            var points = new List<Point>();
            for (int i = 0; i < ship.Length; i++)
            {
                points.Add(ship.Direction switch
                {
                    Core.Enums.Direction.North => new Point(ship.Position.X, ship.Position.Y - i),
                    Core.Enums.Direction.East => new Point(ship.Position.X + i, ship.Position.Y),
                    Core.Enums.Direction.South => new Point(ship.Position.X, ship.Position.Y + i),
                    Core.Enums.Direction.West => new Point(ship.Position.X - i, ship.Position.Y),
                    _ => ship.Position
                });
            }
            return points;
        }
    }
}