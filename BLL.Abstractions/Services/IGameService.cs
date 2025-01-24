using Core.Entities;
using Core.Enums;

namespace BLL.Abstractions.Services
{
    public interface IGameService
    {
        Player CurrentPlayer { get; }
        void SwitchTurn();
        IFieldService GetCurrentPlayerFieldService();
        IFieldService GetOpponentFieldService();
        void Attack(Point targetPosition);
        void Heal(Point targetPosition);
    }
}