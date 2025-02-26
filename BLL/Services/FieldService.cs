using BLL.Abstractions.Helpers;
using BLL.Abstractions.Services;
using Core.Entities;
using Core.Enums;
using DAL.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class FieldService : IFieldService
    {
        private readonly IShipHelper _shipHelper;
        private readonly IRepository<Ship> _shipRepository;
        private readonly Field _field;

        public FieldService(Field field, IShipHelper shipHelper, IRepository<Ship> shipRepository)
        {
            _field = field;
            _shipHelper = shipHelper;
            _shipRepository = shipRepository;
        }

        public void AddShip(Ship ship, int fieldWidth, int fieldHeight)
        {
            string? placementError = IsValidPlacement(ship.Position.X, ship.Position.Y, ship.Direction, ship.Length, fieldWidth, fieldHeight);

            if (placementError != null)
                throw new InvalidOperationException($"Invalid ship placement: {placementError}");

            _field.Ships.Add(ship);
            _shipRepository.Add(ship);
            _shipRepository.SaveChanges();
        }

        public Ship GetShipAt(Point position)
        {
            return _shipRepository.GetAll().Data
                .FirstOrDefault(s => _shipHelper.GetOccupiedPoints(s)
                .Any(p => p.X == position.X && p.Y == position.Y));
        }

        public IEnumerable<Ship> GetAllShips()
        {
            return _shipRepository.GetAll().Data ?? new List<Ship>();
        }

        public Field GetField() => _field;

        public string? IsValidPlacement(int x, int y, Direction direction, int shipLength, int fieldWidth, int fieldHeight)
        {
            var occupiedPoints = new List<Point>();

            for (int i = 0; i < shipLength; i++)
            {
                int projectedX = x, projectedY = y;
                switch (direction)
                {
                    case Direction.North: projectedY -= i; break;
                    case Direction.South: projectedY += i; break;
                    case Direction.East: projectedX += i; break;
                    case Direction.West: projectedX -= i; break;
                }

                occupiedPoints.Add(new Point(projectedX, projectedY));

                if (projectedX < 0 || projectedX >= fieldWidth || projectedY < 0 || projectedY >= fieldHeight)
                    return $"Out of bounds at ({projectedX}, {projectedY})";
            }

            foreach (var existingShip in _shipRepository.GetAll().Data ?? new List<Ship>())
            {
                var existingOccupiedPoints = _shipHelper.GetOccupiedPoints(existingShip).ToList();
                if (occupiedPoints.Intersect(existingOccupiedPoints).Any())
                    return $"Overlapping existing ship at {occupiedPoints.First()}";
            }

            return null;
        }
    }
}