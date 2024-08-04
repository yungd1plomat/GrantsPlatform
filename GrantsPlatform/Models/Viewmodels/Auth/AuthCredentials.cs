using System.Text.Json.Serialization;

namespace GrantsPlatform.Models.Viewmodels.Auth
{
    public class AuthCredentials
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
