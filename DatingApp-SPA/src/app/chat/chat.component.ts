import { Component, OnInit, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  @Input() IsLoggedFromNav: any;

  constructor(private httpCliente: HttpClient) { }

  ngOnInit() {
  }

  connectToServer() {
    if (this.IsLoggedFromNav) {
      console.log('connect');
    }
  }

}
