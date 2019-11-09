using ChatApp.API.JWT.Chaves;
using ChatApp.API.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace ChatApp.API.JWT.Handlers
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtSettings jwtSettings;

        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        private SecurityKey securityKey;

        private SigningCredentials signingCredentials;

        private JwtHeader jwtHeader;

        private PrivateRSA privateRSA;

        private PublicRSA publicRSA;

        public TokenValidationParameters Parameters { get; private set; }

        public JwtHandler(IOptions<JwtSettings> settings, PrivateRSA privateRSA, PublicRSA publicRSA)
        {
            this.privateRSA = privateRSA;
            this.publicRSA = publicRSA;
            
            jwtSettings = settings.Value;

            if (jwtSettings.UseRsa)
            {
                InitializeRSA();
            }
            else
            {
                InitializeHmac();
            }

            InitializeJwtParameters();
        }

        private void InitializeJwtParameters()
        {
            jwtHeader = new JwtHeader(signingCredentials);
            Parameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateActor = false,
                ValidIssuer = jwtSettings.Issuer,
                IssuerSigningKey = securityKey,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ClockSkew = TimeSpan.Zero,
                ValidateTokenReplay = true
            };
        }

        private void InitializeHmac()
        {
            securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.HmacSecretKey));
            signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        private void InitializeRSA()
        {
            /*using(*/
            RSA publicRSAI = RSA.Create();//)
            //{
                publicRSAI.ImportParameters(publicRSA.GetParameters());
                securityKey = new RsaSecurityKey(publicRSAI);
            //}

            if (!jwtSettings.UseRsa)
                return;

            /*using(*/
            RSA privateRSAI = RSA.Create();//)
            //{
                privateRSAI.ImportParameters(privateRSA.GetParameters());
                var privateKeyE = new RsaSecurityKey(privateRSAI);
                signingCredentials = new SigningCredentials(privateKeyE, SecurityAlgorithms.RsaSha256);
            //}
        }

        public Jwt Create(User user)
        {
            if(string.IsNullOrEmpty(user.ImagePath))
            {
                user.ImagePath = "";
            }

            var nowUtc = DateTime.UtcNow;
            var expires = nowUtc.AddMinutes(jwtSettings.expiryMinutes);
            var centuryBegin = new DateTime(1970, 1, 1);
            var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
            var now = (long)(new TimeSpan(nowUtc.Ticks - centuryBegin.Ticks).TotalSeconds);
            var issuer = jwtSettings.Issuer ?? string.Empty;
            var payload = new JwtPayload
            {
                {"sub", user.Id},
                {"user_name", user.Name},
                {"user_photo_path", user.ImagePath},
                {"iss", issuer},
                {"iat", now},
                {"nbf", now},
                {"exp", exp},
                {"jti", Guid.NewGuid().ToString("N")}
            };

            var jwt = new JwtSecurityToken(jwtHeader, payload);
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new Jwt { Token = token, Expires = exp, TokenType = "bearer", Name = user.Name};
        }
    }
}
