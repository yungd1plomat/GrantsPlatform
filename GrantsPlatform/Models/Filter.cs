using System.Text.Json.Serialization;

namespace GrantsPlatform.Models
{
    public class Filter
    {
        [JsonIgnore]
        public int? Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("mapping")]
        public Dictionary<string, Mapping> Mapping { get; set; }
    }
}
