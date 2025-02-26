using BLL.Abstractions.Services;
using BLL.Helper;
using Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class FieldService : IFieldService
    {
        public Field Field { get; }

        public FieldService(Field field)
        {
            Field = field;
        }

        public void AddShip(Ship ship, int fieldWidth, int fieldHeight)
        {
            ValidateShipPlacement(ship, fieldWidth, fieldHeight);
            Field.Ships.Add(ship);
        }

        public Ship GetShipAt(Point position)
        {
            return Field.Ships.FirstOrDefault(s => ShipHelper.GetOccupiedPoints(s).Any(p => p.X == position.X && p.Y == position.Y));
        }

        public IEnumerable<Ship> GetAllShips()
        {
            return Field.Ships;
        }

        private void ValidateShipPlacement(Ship ship, int fieldWidth, int fieldHeight)
        {
            foreach (var point in ShipHelper.GetOccupiedPoints(ship))
            {
                if (point.X < 0 || point.Y < 0 || point.X >= fieldWidth || point.Y >= fieldHeight)
                {
                    throw new InvalidOperationException("Ship placement is out of bounds.");
                }
            }

            foreach (var existingShip in Field.Ships)
            {
                var occupiedPoints = ShipHelper.GetOccupiedPoints(existingShip).ToList();
                var bufferZone = GetBufferZone(occupiedPoints);

                foreach (var point in ShipHelper.GetOccupiedPoints(ship))
                {
                    if (bufferZone.Any(p => p.X == point.X && p.Y == point.Y))
                    {
                        throw new InvalidOperationException("Ship placement violates no-adjacency rule.");
                    }
                }
            }
        }

        private IEnumerable<Point> GetBufferZone(IEnumerable<Point> occupiedPoints)
        {
            var bufferZone = new HashSet<Point>();

            foreach (var point in occupiedPoints)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        bufferZone.Add(new Point(point.X + dx, point.Y + dy));
                    }
                }
            }

            return bufferZone;
        }
    }
}