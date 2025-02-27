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
                int x = ship.Position.X, y = ship.Position.Y;
                switch (ship.Direction)
                {
                    case Core.Enums.Direction.North:
                        y -= i; break;
                    case Core.Enums.Direction.South:
                        y += i; break;
                    case Core.Enums.Direction.East:
                        x += i; break;
                    case Core.Enums.Direction.West:
                        x -= i; break;
                }
                points.Add(new Point(x, y));
            }
            return points;
        }
    }
}