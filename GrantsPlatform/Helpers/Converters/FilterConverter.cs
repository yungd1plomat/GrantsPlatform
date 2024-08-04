using System.Text.Json.Serialization;
using System.Text.Json;
using GrantsPlatform.Models;

namespace GrantsPlatform.Helpers.Converters
{
    public class FilterConverter : JsonConverter<Filter>
    {
        public override Filter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var filter = new Filter();
            filter.Mapping = new Dictionary<string, Mapping>();

            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;
                foreach (var property in root.EnumerateObject())
                {
                    if (property.Name == "title")
                    {
                        filter.Title = property.Value.GetString();
                    }
                    else
                    {
                        foreach (var item in property.Value.EnumerateObject())
                        {
                            var mapping = JsonSerializer.Deserialize<Mapping>(item.Value, options);
                            filter.Mapping.Add(item.Name, mapping);
                        }

                    }
                }
            }

            return filter;
        }

        public override void Write(Utf8JsonWriter writer, Filter value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("title", value.Title);

            writer.WritePropertyName("mapping");
            writer.WriteStartObject();
            foreach (var kvp in value.Mapping)
            {
                writer.WritePropertyName(kvp.Key);
                JsonSerializer.Serialize(writer, kvp.Value, options);
            }
            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}
