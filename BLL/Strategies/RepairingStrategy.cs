using BLL.Abstractions.Strategies;
using Core.Entities;
using Core.Enums;

namespace BLL.Strategies
{
    public class RepairingStrategy : IActionStrategy
    {
        public void ExecuteAction(Ship ship)
        {
            if (ship == null) throw new ArgumentNullException(nameof(ship));

            switch (ship.Health)
            {
                case HealthStage.Critical:
                    ship.Health = HealthStage.Damaged;
                    Console.WriteLine($"Repaired {ship.Type} Ship at {ship.Position} to Damaged state.");
                    break;
                case HealthStage.Damaged:
                    ship.Health = HealthStage.FullHealth;
                    Console.WriteLine($"Repaired {ship.Type} Ship at {ship.Position} to Full Health.");
                    break;
                case HealthStage.FullHealth:
                    Console.WriteLine($"{ship.Type} Ship at {ship.Position} is already at Full Health!");
                    break;
            }
        }
    }
}
