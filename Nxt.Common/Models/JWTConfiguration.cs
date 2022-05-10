namespace Nxt.Common.Models
{
    public class JWTConfiguration
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double TokenExpiryDurationInMinutes { get; set; }
        public double RefreshTokenExpiryDurationInDays { get; set; }
    }
}
