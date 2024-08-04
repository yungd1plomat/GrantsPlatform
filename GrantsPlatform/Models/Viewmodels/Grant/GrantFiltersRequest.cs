using System.Text.Json.Serialization;

namespace GrantsPlatform.Models.Viewmodels.Grant
{
    public class GrantFiltersRequest
    {
        [JsonPropertyName("data")]
        public GrantFilters Filters { get; set; }
    }
}
