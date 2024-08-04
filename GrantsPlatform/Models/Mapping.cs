using System.Text.Json.Serialization;

namespace GrantsPlatform.Models
{
    public class Mapping
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}
