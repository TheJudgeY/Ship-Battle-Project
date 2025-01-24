using Core.Entities;

namespace BLL.Abstractions.Strategies
{
    public interface IActionStrategy
    {
        void ExecuteAction(Ship ship);
    }
}