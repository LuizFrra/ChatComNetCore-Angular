using Microsoft.Extensions.Configuration;
using StatePrinting;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ChatApp.API.JWT.Chaves
{
    public class PrivateRSA
    {
        private byte[] Modulus;

        private byte[] Exponent;

        private byte[] P;

        private byte[] Q;

        private byte[] DP;

        private byte[] DQ;

        private byte[] InverseQ;

        private byte[] D;

        private RSAParameters parameters;

        public PrivateRSA(IConfiguration configuration)
        {
            Modulus = Convert.FromBase64String(configuration.GetSection("privateRSA:Modulus").Value);
            Exponent = Convert.FromBase64String((configuration.GetSection("privateRSA:Exponent").Value));
            P = Convert.FromBase64String((configuration.GetSection("privateRSA:P").Value));
            Q = Convert.FromBase64String((configuration.GetSection("privateRSA:Q").Value));
            DP = Convert.FromBase64String(configuration.GetSection("privateRSA:DP").Value);
            DQ = Convert.FromBase64String((configuration.GetSection("privateRSA:DQ").Value));
            InverseQ = Convert.FromBase64String((configuration.GetSection("privateRSA:InverseQ").Value));
            D = Convert.FromBase64String((configuration.GetSection("privateRSA:D").Value));
        }

        public override string ToString()
        {
            var printer = new Stateprinter();
            return printer.PrintObject(this);
        }

        public RSAParameters GetParameters()
        {
            parameters = new RSAParameters();
            parameters.Modulus = Modulus;
            parameters.Exponent = Exponent;
            parameters.P = P;
            parameters.Q = Q;
            parameters.DP = DP;
            parameters.DQ = DQ;
            parameters.InverseQ = InverseQ;
            parameters.D = D;

            return parameters;
        }

        private byte[] StringToBytes(string Value)
        {
            return Encoding.ASCII.GetBytes(Value);
        }
    }
}
