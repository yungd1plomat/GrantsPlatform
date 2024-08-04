using System.Text.Json.Serialization;

namespace GrantsPlatform.Models.Viewmodels.Grant
{
    public class GrantDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("source_url")]
        public string SourceUrl { get; set; }

        [JsonPropertyName("filter_values")]
        public GrantFilters FilterValues { get; set; }
    }
}
