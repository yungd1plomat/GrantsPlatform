using System.Text.Json.Serialization;
using System.Text.Json;
using GrantsPlatform.Models;
using GrantsPlatform.Models.Viewmodels.Grant;

namespace GrantsPlatform.Helpers.Converters
{
    public class FilterMappingDtoConverter : JsonConverter<FilterMappingDto>
    {
        public override FilterMappingDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var filterMapping = new FilterMappingDto();
            filterMapping.Filters = new Dictionary<string, Filter>();

            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;
                foreach (var property in root.EnumerateObject())
                {
                    var filter = JsonSerializer.Deserialize<Filter>(property.Value, options);
                    filterMapping.Filters.Add(property.Name, filter);
                }
            }

            return filterMapping;
        }

        public override void Write(Utf8JsonWriter writer, FilterMappingDto value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (var kvp in value.Filters)
            {
                writer.WritePropertyName(kvp.Key);
                JsonSerializer.Serialize(writer, kvp.Value, options);
            }

            writer.WriteEndObject();
        }
    }
}
