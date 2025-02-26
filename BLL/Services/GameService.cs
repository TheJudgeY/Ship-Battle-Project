using BLL.Abstractions.Services;
using BLL.Strategies;
using Core.Entities;
using Core.Enums;
using Core.Utilities;

namespace BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IFieldService _player1FieldService;
        private readonly IFieldService _player2FieldService;
        private Player _currentPlayer;

        public GameService(IFieldService player1FieldService, IFieldService player2FieldService)
        {
            _player1FieldService = player1FieldService;
            _player2FieldService = player2FieldService;
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
                return OperationResult<bool>.Failure("Miss! No ship at the target position.");

            var targetShip = targetShipResult.Data;
            var shootingStrategy = new ShootingStrategy(opponentFieldService);
            shootingStrategy.ExecuteAction(targetShip);

            var remainingShips = opponentFieldService.GetAllShips();
            if (!remainingShips.IsSuccess || !remainingShips.Data.Any())
            {
                return OperationResult<bool>.Success(true, $"{_currentPlayer} wins! All opponent ships destroyed.");
            }

            return OperationResult<bool>.Success(true);
        }

        public OperationResult<bool> Heal(Point targetPosition)
        {
            var playerFieldService = GetCurrentPlayerFieldService();
            var targetShipResult = playerFieldService.GetShipAt(targetPosition);

            if (!targetShipResult.IsSuccess)
                return OperationResult<bool>.Failure("No ship at the target position.");

            var targetShip = targetShipResult.Data;
            var repairingStrategy = new RepairingStrategy();
            repairingStrategy.ExecuteAction(targetShip);

            return OperationResult<bool>.Success(true);
        }
    }
}
