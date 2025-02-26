using Core.Utilities;

namespace DAL.Abstractions
{
    public interface IRepository<T>
    {
        OperationResult<bool> Add(T entity);
        OperationResult<T> Get(Func<T, bool> predicate);
        OperationResult<List<T>> GetAll();
        OperationResult<bool> Remove(T entity);
        OperationResult<bool> SaveChanges();
    }
}
