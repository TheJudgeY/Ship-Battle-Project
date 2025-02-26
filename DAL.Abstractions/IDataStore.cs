using Core.Utilities;

namespace DAL.Abstractions
{
    public interface IDataStore<T>
    {
        OperationResult<T> Load();
        OperationResult<bool> Save(T data);
    }
}
