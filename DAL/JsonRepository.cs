using DAL.Abstractions;
using Core.Utilities;

namespace DAL.Repositories
{
    public class JsonRepository<T> : IRepository<T>
    {
        private readonly IDataStore<T> _dataStore;
        private readonly List<T> _entities;

        public JsonRepository(IDataStore<T> dataStore)
        {
            _dataStore = dataStore;

            var loadResult = _dataStore.Load();
            _entities = loadResult.IsSuccess ? loadResult.Data : new List<T>();
        }

        public OperationResult<bool> Add(T entity)
        {
            if (_entities.Contains(entity))
            {
                return OperationResult<bool>.Failure("Entity already exists.");
            }

            _entities.Add(entity);
            return _dataStore.Save(_entities);
        }

        public OperationResult<T> Get(Func<T, bool> predicate)
        {
            var entity = _entities.FirstOrDefault(predicate);
            return entity != null
                ? OperationResult<T>.Success(entity)
                : OperationResult<T>.Failure("Entity not found.");
        }

        public OperationResult<List<T>> GetAll()
        {
            return OperationResult<List<T>>.Success(_entities);
        }

        public OperationResult<bool> Remove(T entity)
        {
            if (!_entities.Contains(entity))
            {
                return OperationResult<bool>.Failure("Entity not found.");
            }

            _entities.Remove(entity);
            return _dataStore.Save(_entities);
        }

        public OperationResult<bool> SaveChanges()
        {
            return _dataStore.Save(_entities);
        }
    }
}
