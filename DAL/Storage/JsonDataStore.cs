using System.Text.Json;
using System.Text.Json.Serialization;
using DAL.Abstractions;
using Core.Utilities;
using Core.Entities;
using Core.Converters;

namespace DAL.Repositories
{
    public class JsonDataStore<T> : IDataStore<Dictionary<string, List<Ship>>>
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public JsonDataStore(string filePath)
        {
            _filePath = filePath;
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter(), new ShipJsonConverter() }
            };
        }

        public OperationResult<Dictionary<string, List<Ship>>> Load()
        {
            if (!File.Exists(_filePath))
                return OperationResult<Dictionary<string, List<Ship>>>.Success(new Dictionary<string, List<Ship>>());

            try
            {
                var jsonData = File.ReadAllText(_filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, List<Ship>>>(jsonData, _jsonOptions)
                           ?? new Dictionary<string, List<Ship>>();

                return OperationResult<Dictionary<string, List<Ship>>>.Success(data);
            }
            catch (Exception ex)
            {
                return OperationResult<Dictionary<string, List<Ship>>>.Failure($"Failed to load data: {ex.Message}");
            }
        }

        public OperationResult<bool> Save(Dictionary<string, List<Ship>> data)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(data, _jsonOptions);
                File.WriteAllText(_filePath, jsonData);
                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Failed to save data: {ex.Message}");
            }
        }
    }
}