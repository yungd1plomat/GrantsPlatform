using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GrantsPlatform.Models
{
    public class JwtOptions
    {
        private string secretKey { get; set; }

        public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int ExpireDays { get; set; }


        public JwtOptions(string secretKey, string issuer, string audience, int expireDays)
        {
            this.secretKey = secretKey;
            this.Issuer = issuer;
            this.Audience = audience;
            this.ExpireDays = expireDays;
        }

    }
}
