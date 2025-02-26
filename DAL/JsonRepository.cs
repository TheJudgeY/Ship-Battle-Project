using DAL.Abstractions;
using System.Text.Json;

namespace DAL
{
    public class JsonRepository<T> : IRepository<T>
    {
        private readonly string _filePath;
        private readonly List<T> _entities;

        public JsonRepository(string filePath)
        {
            _filePath = filePath;

            if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                _entities = JsonSerializer.Deserialize<List<T>>(jsonData) ?? new List<T>();
            }
            else
            {
                _entities = new List<T>();
            }
        }

        public void Add(T entity)
        {
            if (_entities.Contains(entity))
            {
                throw new InvalidOperationException("Entity already exists.");
            }

            _entities.Add(entity);
        }

        public T Get(Func<T, bool> predicate)
        {
            return _entities.FirstOrDefault(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.AsReadOnly();
        }

        public void Remove(T entity)
        {
            if (!_entities.Contains(entity))
            {
                throw new InvalidOperationException("Entity not found.");
            }

            _entities.Remove(entity);
        }

        public void SaveChanges()
        {
            var jsonData = JsonSerializer.Serialize(_entities, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }

        private void ReloadData()
        {
            if (File.Exists(_filePath))
            {
                var jsonData = File.ReadAllText(_filePath);
                _entities.Clear();
                _entities.AddRange(JsonSerializer.Deserialize<List<T>>(jsonData) ?? new List<T>());
            }
        }
    }
}
