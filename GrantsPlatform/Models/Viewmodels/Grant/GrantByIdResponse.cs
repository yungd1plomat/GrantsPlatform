using System.Text.Json.Serialization;

namespace GrantsPlatform.Models.Viewmodels.Grant
{
    public class GrantByIdResponse
    {
        [JsonPropertyName("grant")]
        public GrantDto Grant { get; set; }

        [JsonPropertyName("filters_mapping")]
        public FilterMappingDto FiltersMapping { get; set; }

        [JsonPropertyName("filters_order")]
        public IEnumerable<string> FiltersOrder { get; set; }
    }
}
