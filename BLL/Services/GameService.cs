using BLL.Abstractions.Services;
using BLL.Strategies;
using Core.Entities;
using Core.Enums;
using Core.Utilities;
using DAL.Abstractions;

namespace BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IFieldService _player1FieldService;
        private readonly IFieldService _player2FieldService;
        private readonly IDataStore<Dictionary<string, List<Ship>>> _dataStore;
        private Player _currentPlayer;

        public GameService(IFieldService player1FieldService, IFieldService player2FieldService, IDataStore<Dictionary<string, List<Ship>>> dataStore)
        {
            _player1FieldService = player1FieldService;
            _player2FieldService = player2FieldService;
            _dataStore = dataStore;
            _currentPlayer = Player.Player1;
        }

        public void SetCurrentPlayer(Player player)
        {
            _currentPlayer = player;
        }

        public void SwitchTurn()
        {
            _currentPlayer = _currentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
        }

        public Player GetCurrentPlayer()
        {
            return _currentPlayer;
        }

        public IFieldService GetCurrentPlayerFieldService()
        {
            return _currentPlayer == Player.Player1 ? _player1FieldService : _player2FieldService;
        }

        public IFieldService GetOpponentFieldService()
        {
            return _currentPlayer == Player.Player1 ? _player2FieldService : _player1FieldService;
        }

        public OperationResult<bool> Attack(Point targetPosition)
        {
            var opponentFieldService = GetOpponentFieldService();
            var targetShipResult = opponentFieldService.GetShipAt(targetPosition);

            if (!targetShipResult.IsSuccess)
                return OperationResult<bool>.Failure(targetShipResult.Message);

            var targetShip = targetShipResult.Data;
            var shootingStrategy = new ShootingStrategy(opponentFieldService);

            var shootingResult = shootingStrategy.ExecuteAction(targetShip);
            if (!shootingResult.IsSuccess)
                return shootingResult;

            var saveResult = opponentFieldService.SaveChanges();
            if (!saveResult.IsSuccess)
                return saveResult;

            var remainingShips = opponentFieldService.GetAllShips();
            if (!remainingShips.IsSuccess)
                return OperationResult<bool>.Failure(remainingShips.Message);

            if (!remainingShips.Data.Any())
            {
                ClearGameData();
                return OperationResult<bool>.Success(true, $"{_currentPlayer} wins! All opponent ships destroyed.");
            }

            return OperationResult<bool>.Success(true);
        }

        public OperationResult<bool> Heal(Point targetPosition)
        {
            var playerFieldService = GetCurrentPlayerFieldService();
            var targetShipResult = playerFieldService.GetShipAt(targetPosition);

            if (!targetShipResult.IsSuccess)
                return OperationResult<bool>.Failure(targetShipResult.Message);

            var targetShip = targetShipResult.Data;
            var repairingStrategy = new RepairingStrategy();

            var healingResult = repairingStrategy.ExecuteAction(targetShip);
            if (!healingResult.IsSuccess)
                return OperationResult<bool>.Failure(healingResult.Message);

            var saveResult = playerFieldService.SaveChanges();
            if (!saveResult.IsSuccess)
                return saveResult;

            return OperationResult<bool>.Success(true);
        }

        public void ClearGameData()
        {
            _dataStore.Save(new Dictionary<string, List<Ship>>());

            _player1FieldService.GetField().Ships.Clear();
            _player2FieldService.GetField().Ships.Clear();

            foreach (var ship in _player1FieldService.GetAllShips().Data?.ToList() ?? new List<Ship>())
                _player1FieldService.RemoveShip(ship.Position);

            foreach (var ship in _player2FieldService.GetAllShips().Data?.ToList() ?? new List<Ship>())
                _player2FieldService.RemoveShip(ship.Position);
        }
    }
}
