using Microsoft.Extensions.Configuration;
using StatePrinting;
using System;
using System.Security.Cryptography;

namespace DatingApp.API.JWT.Chaves
{
    public class PublicRSA
    {
        private byte[] Modulus;

        private byte[] Exponent;

        public PublicRSA(IConfiguration configuration)
        {
            Modulus = Convert.FromBase64String(configuration.GetSection("publicRSA:Modulus").Value);
            Exponent = Convert.FromBase64String(configuration.GetSection("publicRSA:Exponent").Value);
        }

        public override string ToString()
        {
            var printer = new Stateprinter();
            return printer.PrintObject(this);
        }

        public RSAParameters GetParameters()
        {
            RSAParameters parameters = new RSAParameters();
            parameters.Modulus = Modulus;
            parameters.Exponent = Exponent;

            return parameters;
        }
    }
}
