using System.Text.Json;
using System.Text.Json.Serialization;

namespace GrantsPlatform.Models.Viewmodels.Grant
{
    public class GrantFilters
    {
        [JsonExtensionData]
        public Dictionary<string, JsonElement> Properties { get; set; }
    }
}
