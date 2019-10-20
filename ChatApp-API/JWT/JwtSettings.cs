namespace ChatApp.API.JWT
{
    public class JwtSettings
    {
        public string HmacSecretKey { get; set; }
        public int expiryMinutes { get; set; }
        public string Issuer { get; set; }
        public bool UseRsa { get; set; }
    }
}
