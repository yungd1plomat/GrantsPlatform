using System.Text.Json.Serialization;

namespace GrantsPlatform.Models.Viewmodels.Grant
{
    public class PaginationMeta
    {
        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
    }
}
