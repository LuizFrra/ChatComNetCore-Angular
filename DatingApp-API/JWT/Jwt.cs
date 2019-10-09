namespace DatingApp.API.JWT
{
    public class Jwt
    {
        public string Token { get; set; }
        public long Expires { get; set; }
        public string TokenType { get; set; }
    }
}
