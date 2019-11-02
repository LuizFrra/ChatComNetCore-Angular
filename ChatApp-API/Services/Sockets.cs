using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Threading;

namespace DatingApp.API.Services
{
    public class Sockets
    {
        private IList<WebSocket> Conexoes { get; set; }

        private int NumeroConexoes { get; set; }

        public Sockets()
        {
            Conexoes = new List<WebSocket>();
            NumeroConexoes = 0;
        }

        public void AdicionarSocket(WebSocket conexao)
        {
            Conexoes.Add(conexao);
            NumeroConexoes = Conexoes.Count;
        }

        public async Task RepassarMensagem(Mensagem mensagem, WebSocketMessageType typeMessage, bool EndOfMessage)
        {
            var json = mensagem.pegarObjetoEmBytes();
            
            foreach(var conexao in Conexoes)
            {
                await conexao.SendAsync(new ArraySegment<byte>(json, 0, json.Length), typeMessage, EndOfMessage, CancellationToken.None);
            }

            await Task.CompletedTask;
        }

        public bool RemoverSocket(WebSocket socket)
        {
            return Conexoes.Remove(socket);
        }
    }
}
