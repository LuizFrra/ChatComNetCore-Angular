using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Models
{
    public class Mensagem
    {
        public string Texto { get; set; }

        public string Imagem { get; set; }

        public string Apelido { get; set; }

        public Mensagem(byte[] texto, int tamanhoTexto, string imagem, string apelido)
        {
            Texto = System.Text.Encoding.UTF8.GetString(texto, 0, tamanhoTexto).Replace("\\n", "");
            Imagem = imagem;
            Apelido = apelido;
        }

        public byte[] pegarObjetoEmBytes()
        {
            return System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }
    }
}
