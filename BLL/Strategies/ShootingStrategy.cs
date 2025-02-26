using BLL.Abstractions.Services;
using BLL.Abstractions.Strategies;
using Core.Entities;
using Core.Enums;

namespace BLL.Strategies
{
    public class ShootingStrategy : IActionStrategy
    {
        private readonly IFieldService _fieldService;

        public ShootingStrategy(IFieldService fieldService)
        {
            _fieldService = fieldService;
        }

        public void ExecuteAction(Ship ship)
        {
            if (ship == null) throw new ArgumentNullException(nameof(ship));

            switch (ship.Health)
            {
                case HealthStage.FullHealth:
                    ship.Health = HealthStage.Damaged;
                    Console.WriteLine($"{ship.Type} Ship at {ship.Position} has been damaged!");
                    break;
                case HealthStage.Damaged:
                    ship.Health = HealthStage.Critical;
                    Console.WriteLine($"{ship.Type} Ship at {ship.Position} is now in critical condition!");
                    break;
                case HealthStage.Critical:
                    Console.WriteLine($"{ship.Type} Ship at {ship.Position} has been destroyed!");
                    _fieldService.GetAllShips().Data.ToList().Remove(ship);
                    break;
            }
        }
    }
}