using BLL.Abstractions.Helpers;
using BLL.Abstractions.Services;
using Core.Entities;
using Core.Enums;
using Core.Utilities;
using DAL.Abstractions;

namespace BLL.Services
{
    public class FieldService : IFieldService
    {
        private readonly Player _currentPlayer;
        private readonly IShipHelper _shipHelper;
        private readonly IRepository<Ship> _shipRepository;
        private readonly Field _field;

        public FieldService(Player currentPlayer, Field field, IShipHelper shipHelper, IRepository<Ship> shipRepository)
        {
            _currentPlayer = currentPlayer;
            _field = field;
            _shipHelper = shipHelper;
            _shipRepository = shipRepository;

            var loadedShips = _shipRepository.GetAll(_currentPlayer);
            if (loadedShips.IsSuccess && loadedShips.Data != null)
            {
                _field.Ships.Clear();
                _field.Ships.AddRange(loadedShips.Data);
            }
        }

        public OperationResult<bool> AddShip(Ship ship, int fieldWidth, int fieldHeight)
        {
            var placementCheck = IsValidPlacement(ship.Position.X, ship.Position.Y, ship.Direction, ship.Length, fieldWidth, fieldHeight);

            if (!placementCheck.IsSuccess)
                return OperationResult<bool>.Failure($"Invalid ship placement: {placementCheck.Message}");

            var saveResult = _shipRepository.Add(ship, _currentPlayer);

            if (!saveResult.IsSuccess)
                return OperationResult<bool>.Failure($"Failed to add ship: {saveResult.Message}");

            _field.Ships.Add(ship);
            return _shipRepository.SaveChanges();
        }

        public OperationResult<Ship> GetShipAt(Point position)
        {
            var ships = _shipRepository.GetAll(_currentPlayer).Data ?? new List<Ship>();

            var foundShip = ships.FirstOrDefault(s => _shipHelper.GetOccupiedPoints(s)
                .Any(p => p.X == position.X && p.Y == position.Y));

            return foundShip != null
                ? OperationResult<Ship>.Success(foundShip)
                : OperationResult<Ship>.Failure("No ship found at the given position.");
        }

        public OperationResult<IEnumerable<Ship>> GetAllShips()
        {
            var ships = _shipRepository.GetAll(_currentPlayer);
            return ships.IsSuccess
                ? OperationResult<IEnumerable<Ship>>.Success(ships.Data ?? new List<Ship>())
                : OperationResult<IEnumerable<Ship>>.Failure("Failed to retrieve ships.");
        }

        public Field GetField() => _field;

        public OperationResult<bool> RemoveShip(Point position)
        {
            var shipResult = GetShipAt(position);
            if (!shipResult.IsSuccess)
                return OperationResult<bool>.Failure(shipResult.Message);

            var ship = shipResult.Data;
            var removeResult = _shipRepository.Remove(ship, _currentPlayer);

            if (!removeResult.IsSuccess)
                return OperationResult<bool>.Failure($"Failed to remove ship: {removeResult.Message}");

            _field.Ships.Remove(ship);
            return _shipRepository.SaveChanges();
        }

        public OperationResult<bool> IsValidPlacement(int x, int y, Direction direction, int shipLength, int fieldWidth, int fieldHeight)
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
                    return OperationResult<bool>.Failure($"Out of bounds at ({projectedX}, {projectedY})");
            }

            foreach (var existingShip in _shipRepository.GetAll(_currentPlayer).Data ?? new List<Ship>())
            {
                var existingOccupiedPoints = _shipHelper.GetOccupiedPoints(existingShip).ToList();
                if (occupiedPoints.Intersect(existingOccupiedPoints).Any())
                    return OperationResult<bool>.Failure($"Overlapping existing ship at {occupiedPoints.First()}");
            }

            return OperationResult<bool>.Success(true);
        }
    }
}