using Core.Enums;
using Core.Utilities;

namespace DAL.Abstractions
{
    public interface IRepository<T>
    {
        OperationResult<bool> Add(T entity, Player player);
        OperationResult<T> Get(Func<T, bool> predicate, Player player);
        OperationResult<List<T>> GetAll(Player player);
        OperationResult<bool> Remove(T entity, Player player);
        OperationResult<bool> SaveChanges();
    }
}
