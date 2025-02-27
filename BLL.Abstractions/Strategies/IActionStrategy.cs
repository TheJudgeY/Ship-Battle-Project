using Core.Entities;
using Core.Utilities;

namespace BLL.Abstractions.Strategies
{
    public interface IActionStrategy
    {
        OperationResult<bool> ExecuteAction(Ship ship);
    }
}