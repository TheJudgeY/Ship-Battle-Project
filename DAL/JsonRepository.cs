using Core.Enums;
using Core.Utilities;
using DAL.Abstractions;

namespace DAL.Repositories
{
    public class JsonRepository<T> : IRepository<T> where T : class
    {
        private readonly IDataStore<Dictionary<string, List<T>>> _dataStore;
        private Dictionary<string, List<T>> _playerData;

        public JsonRepository(IDataStore<Dictionary<string, List<T>>> dataStore)
        {
            _dataStore = dataStore;
            var loadResult = _dataStore.Load();
            _playerData = loadResult.IsSuccess && loadResult.Data != null
                ? loadResult.Data
                : new Dictionary<string, List<T>>();

            if (!_playerData.ContainsKey(Player.Player1.ToString()))
                _playerData[Player.Player1.ToString()] = new List<T>();

            if (!_playerData.ContainsKey(Player.Player2.ToString()))
                _playerData[Player.Player2.ToString()] = new List<T>();
        }

        public OperationResult<bool> Add(T entity, Player player)
        {
            string playerKey = player.ToString();
            if (!_playerData.ContainsKey(playerKey))
                _playerData[playerKey] = new List<T>();

            if (_playerData[playerKey].Contains(entity))
                return OperationResult<bool>.Failure("Entity already exists.");

            _playerData[playerKey].Add(entity);
            return SaveChanges();
        }

        public OperationResult<T> Get(Func<T, bool> predicate, Player player)
        {
            string playerKey = player.ToString();
            if (!_playerData.ContainsKey(playerKey))
                return OperationResult<T>.Failure("No data found for player.");

            var entity = _playerData[playerKey].FirstOrDefault(predicate);
            return entity != null
                ? OperationResult<T>.Success(entity)
                : OperationResult<T>.Failure("Entity not found.");
        }

        public OperationResult<List<T>> GetAll(Player player)
        {
            string playerKey = player.ToString();
            if (!_playerData.ContainsKey(playerKey))
                return OperationResult<List<T>>.Failure("No data found for player.");

            return OperationResult<List<T>>.Success(new List<T>(_playerData[playerKey]));
        }

        public OperationResult<bool> Remove(T entity, Player player)
        {
            string playerKey = player.ToString();
            if (!_playerData.ContainsKey(playerKey) || !_playerData[playerKey].Contains(entity))
                return OperationResult<bool>.Failure("Entity not found.");

            _playerData[playerKey].Remove(entity);
            return SaveChanges();
        }

        public OperationResult<bool> SaveChanges()
        {
            return _dataStore.Save(_playerData);
        }
    }
}