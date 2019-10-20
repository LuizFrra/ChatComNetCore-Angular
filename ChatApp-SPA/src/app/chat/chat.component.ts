import { Component, OnInit, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ChatService } from '../_services/chat.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  @Input() IsLoggedFromNav: any;

  isConnected: boolean;

  constructor(private httpCliente: HttpClient, private chatService: ChatService) { }

  ngOnInit() {
  }

  connectToServer() {
    if (this.IsLoggedFromNav) {
      console.log('connect');
      this.chatService.connectToServer();
      this.isConnected = true;
    }
  }
}
