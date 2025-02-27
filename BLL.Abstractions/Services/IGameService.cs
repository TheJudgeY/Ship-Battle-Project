using Core.Entities;
using Core.Enums;
using Core.Utilities;

namespace BLL.Abstractions.Services
{
    public interface IGameService
    {
        void SetCurrentPlayer(Player player);
        void SwitchTurn();
        Player GetCurrentPlayer();
        IFieldService GetCurrentPlayerFieldService();
        IFieldService GetOpponentFieldService();
        OperationResult<bool> Attack(Point targetPosition);
        OperationResult<bool> Heal(Point targetPosition);
        void ClearGameData();
    }
}