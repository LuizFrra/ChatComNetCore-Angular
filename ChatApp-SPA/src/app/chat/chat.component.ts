import { Component, OnInit, Input, Renderer2, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ChatService } from '../_services/chat.service';
import { Message } from '../Models/Message';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  @Input() IsLoggedFromNav: any;

  isConnected: boolean;
  textarea: string;
  messages: Array<Message>;

  constructor(private httpCliente: HttpClient, private chatService: ChatService) { 
    this.messages = new Array<Message>();
  }

  ngOnInit() {
  }

  connectToServer() {
    if (this.IsLoggedFromNav) {
      console.log(this.isConnected);
      let socket: WebSocket;
      console.log('Conectando-se ...');
      if (this.isConnected === undefined) {
        socket = this.chatService.connectToServer();
      }
      if (socket != null) {
        socket.onopen = (event) => {
          this.isConnected = true;
          this.receivedMessage(socket);
          console.log('Conexao Com o Servidor Estabelecida.');
        };
        socket.onclose = (event) => {
          console.log('Conexao Com o Servidor Encerrada.');
          this.isConnected = undefined;
        };
      }
    }
  }

  sendMessage() {
    console.log('Enviando Mensagem.');
    this.chatService.sendMessage(this.textarea);
    this.textarea = '';
  }

  receivedMessage(socket: WebSocket) {
    socket.onmessage = (menssage) => {
      console.log(menssage.data);
      const message: Message = JSON.parse(menssage.data) as Message;
      this.messages.unshift(message);
      if (message.Apelido === localStorage.getItem('username'))
      {
        message.IsMySelf = true;
      }
      console.log(this.messages);
    };
  }
}
