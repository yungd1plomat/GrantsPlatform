using System.Text.Json.Serialization;

namespace GrantsPlatform.Models.Viewmodels.Auth
{
    public class LoginRequest
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
