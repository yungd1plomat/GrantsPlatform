using GrantsPlatform.Helpers.Converters;
using System.Text.Json.Serialization;

namespace GrantsPlatform.Models.Viewmodels.Grant
{
    public class FilterMappingDto
    {
        [JsonExtensionData]
        public Dictionary<string, Filter> Filters { get; set; }
    }
}
