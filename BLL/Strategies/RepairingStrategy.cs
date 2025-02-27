using BLL.Abstractions.Strategies;
using Core.Entities;
using Core.Enums;
using Core.Utilities;

namespace BLL.Strategies
{
    public class RepairingStrategy : IActionStrategy
    {
        public OperationResult<bool> ExecuteAction(Ship ship)
        {
            if (ship == null)
                return OperationResult<bool>.Failure("Invalid ship: Ship is null.");

            switch (ship.Health)
            {
                case HealthStage.Critical:
                    ship.Health = HealthStage.Damaged;
                    return OperationResult<bool>.Success(true);

                case HealthStage.Damaged:
                    ship.Health = HealthStage.FullHealth;
                    return OperationResult<bool>.Success(true);

                case HealthStage.FullHealth:
                    return OperationResult<bool>.Failure("Ship is already at Full Health.");
            }

            return OperationResult<bool>.Failure("Unknown repair issue.");
        }
    }
}