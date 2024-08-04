using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace GrantsPlatform.Models.Viewmodels.Grant
{

    public class GrantsResponse
    {
        [JsonPropertyName("grants")]
        public IEnumerable<GrantDto> Grants { get; set; }

        [JsonPropertyName("filters_mapping")]
        public FilterMappingDto FiltersMapping { get; set; }

        [JsonPropertyName("filters_order")]
        public IEnumerable<string> FiltersOrder { get; set; }

        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }
    }
}
