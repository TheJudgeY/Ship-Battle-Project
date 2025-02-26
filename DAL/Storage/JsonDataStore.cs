using System.Text.Json;
using System.Text.Json.Serialization;
using DAL.Abstractions;
using Core.Utilities;
using DAL.Converters;
using Core.Entities;

namespace DAL.Storage
{
    public class JsonDataStore<T> : IDataStore<T>
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public JsonDataStore(string filePath)
        {
            _filePath = filePath;

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters =
                {
                    new JsonStringEnumConverter(),
                    typeof(T) == typeof(Ship) ? new ShipJsonConverter() : null
                }
            };
        }

        public OperationResult<List<T>> Load()
        {
            if (!File.Exists(_filePath))
            {
                return OperationResult<List<T>>.Success(new List<T>());
            }

            try
            {
                var jsonData = File.ReadAllText(_filePath);
                var data = JsonSerializer.Deserialize<List<T>>(jsonData, _jsonOptions);
                return OperationResult<List<T>>.Success(data ?? new List<T>());
            }
            catch (Exception ex)
            {
                return OperationResult<List<T>>.Failure($"Error loading data: {ex.Message}");
            }
        }

        public OperationResult<bool> Save(List<T> entities)
        {
            try
            {
                string tempFile = _filePath + ".tmp";
                string jsonData = JsonSerializer.Serialize(entities, _jsonOptions);

                File.WriteAllText(tempFile, jsonData);
                File.Replace(tempFile, _filePath, null);

                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Error saving data: {ex.Message}");
            }
        }
    }
}
