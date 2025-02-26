using Core.Utilities;

namespace DAL.Abstractions
{
    public interface IDataStore<T>
    {
        OperationResult<List<T>> Load();
        OperationResult<bool> Save(List<T> entities);
    }
}