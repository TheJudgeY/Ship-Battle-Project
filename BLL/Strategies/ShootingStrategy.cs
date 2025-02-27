using BLL.Abstractions.Services;
using BLL.Abstractions.Strategies;
using Core.Entities;
using Core.Enums;
using Core.Utilities;

namespace BLL.Strategies
{
    public class ShootingStrategy : IActionStrategy
    {
        private readonly IFieldService _fieldService;

        public ShootingStrategy(IFieldService fieldService)
        {
            _fieldService = fieldService;
        }

        public OperationResult<bool> ExecuteAction(Ship ship)
        {
            if (ship == null)
                return OperationResult<bool>.Failure("Ship does not exist at the given location.");

            switch (ship.Health)
            {
                case HealthStage.FullHealth:
                    ship.Health = HealthStage.Damaged;
                    return OperationResult<bool>.Success(true);

                case HealthStage.Damaged:
                    ship.Health = HealthStage.Critical;
                    return OperationResult<bool>.Success(true);

                case HealthStage.Critical:
                    var removalResult = _fieldService.RemoveShip(ship.Position);
                    if (!removalResult.IsSuccess)
                        return OperationResult<bool>.Failure($"Failed to remove ship: {removalResult.Message}");

                    return OperationResult<bool>.Success(true);

                default:
                    return OperationResult<bool>.Failure("Unexpected ship health state.");
            }
        }
    }
}