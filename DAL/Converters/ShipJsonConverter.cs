using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Entities;

namespace DAL.Converters
{
    public class ShipJsonConverter : JsonConverter<Ship>
    {
        public override Ship Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;
                if (!root.TryGetProperty("Type", out JsonElement typeElement))
                {
                    throw new JsonException("Missing 'Type' property in JSON.");
                }

                string shipType = typeElement.GetString()?.ToLower() ?? throw new JsonException("Invalid ship type.");
                return shipType switch
                {
                    "military" => JsonSerializer.Deserialize<MilitaryShip>(root.GetRawText(), options)!,
                    "auxiliary" => JsonSerializer.Deserialize<AuxiliaryShip>(root.GetRawText(), options)!,
                    "mixed" => JsonSerializer.Deserialize<MixedShip>(root.GetRawText(), options)!,
                    _ => throw new JsonException($"Unknown ship type: {shipType}")
                };
            }
        }

        public override void Write(Utf8JsonWriter writer, Ship value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
        }
    }
}