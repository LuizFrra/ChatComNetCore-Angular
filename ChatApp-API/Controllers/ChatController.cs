using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DatingApp.API.Models;
using DatingApp.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChatApp_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private Sockets sockets;

        public ChatController(Sockets _sockets)
        {
            sockets = _sockets;
        }

        [HttpGet("/api/chat")]
        public async Task Index()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var connection = await HttpContext.WebSockets.AcceptWebSocketAsync("jwt");
                
                sockets.AdicionarSocket(connection);
                
                await Echo(connection);
            }
        }

        private async Task Echo(WebSocket socket) 
        {
            var buffer = new byte[1000];

            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            var userName = HttpContext.User.FindFirst("user_name").Value;
            var imagePath = HttpContext.User.FindFirst("user_photo_path").Value;
            Mensagem mensagem = null;
            
            while (!result.CloseStatus.HasValue)
            {
                //await socket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                if (result.Count > 1)
                {
                    mensagem = new Mensagem(buffer, result.Count, imagePath, userName);
                    await sockets.RepassarMensagem(mensagem, result.MessageType, result.EndOfMessage);
                }

                result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            
            sockets.RemoverSocket(socket);
            
            await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}