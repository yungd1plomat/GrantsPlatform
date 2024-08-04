using System.Text.Json.Serialization;

namespace GrantsPlatform.Models.Viewmodels
{
    public class ErrorResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
