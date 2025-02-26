using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Entities;
using Core.Enums;

namespace Core.Converters
{
    public class ShipJsonConverter : JsonConverter<Ship>
    {
        public override Ship Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var jsonDoc = JsonDocument.ParseValue(ref reader);
            var root = jsonDoc.RootElement;

            var type = root.GetProperty("Type").GetString();
            var positionElement = root.GetProperty("Position");

            int posX = positionElement.GetProperty("X").GetInt32();
            int posY = positionElement.GetProperty("Y").GetInt32();

            var position = new Point(posX, posY);
            var direction = Enum.Parse<Direction>(root.GetProperty("Direction").GetString());

            return type switch
            {
                "Military" => new MilitaryShip(position, direction),
                "Auxiliary" => new AuxiliaryShip(position, direction),
                "Mixed" => new MixedShip(position, direction),
                _ => throw new JsonException("Unknown ship type")
            };
        }

        public override void Write(Utf8JsonWriter writer, Ship value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options);
        }
    }
}