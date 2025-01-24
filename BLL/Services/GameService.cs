using BLL.Abstractions.Services;
using BLL.Strategies;
using Core.Entities;
using Core.Enums;

namespace BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IFieldService _player1FieldService;
        private readonly IFieldService _player2FieldService;

        public Player CurrentPlayer { get; private set; }

        public GameService(IFieldService player1FieldService, IFieldService player2FieldService)
        {
            _player1FieldService = player1FieldService;
            _player2FieldService = player2FieldService;
            CurrentPlayer = Player.Player1;
        }

        public void SwitchTurn()
        {
            CurrentPlayer = CurrentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
        }

        public IFieldService GetCurrentPlayerFieldService()
        {
            return CurrentPlayer == Player.Player1 ? _player1FieldService : _player2FieldService;
        }

        public IFieldService GetOpponentFieldService()
        {
            return CurrentPlayer == Player.Player1 ? _player2FieldService : _player1FieldService;
        }

        public void Attack(Point targetPosition)
        {
            var opponentFieldService = GetOpponentFieldService();

            var targetShip = opponentFieldService.GetShipAt(targetPosition);
            if (targetShip == null)
            {
                Console.WriteLine("Miss! No ship at the target position.");
                return;
            }

            var shootingStrategy = new ShootingStrategy(opponentFieldService);
            shootingStrategy.ExecuteAction(targetShip);

            if (!opponentFieldService.GetAllShips().Any())
            {
                Console.WriteLine($"{CurrentPlayer} wins! All opponent ships destroyed.");
                Environment.Exit(0);
            }
        }

        public void Heal(Point targetPosition)
        {
            var playerFieldService = GetCurrentPlayerFieldService();

            var targetShip = playerFieldService.GetShipAt(targetPosition);
            if (targetShip == null)
            {
                Console.WriteLine("No ship at the target position.");
                return;
            }

            var repairingStrategy = new RepairingStrategy();
            repairingStrategy.ExecuteAction(targetShip);
        }
    }
}
