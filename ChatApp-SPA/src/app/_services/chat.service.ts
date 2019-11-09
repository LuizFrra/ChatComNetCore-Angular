import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Location } from '@angular/common';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
socket: WebSocket;

constructor(private http: HttpClient, location: Location) { }

  connectToServer() {
    const apiUrl = environment.apiUrl.replace('http://', '');
    const protocolo = location.protocol === 'https' ? 'wss://' : 'ws://';
    const porta = environment.port ? (':' + environment.port) : '';
    const urlConexao = protocolo + apiUrl + porta + '/api/Chat';
    const token = localStorage.getItem('token');
    if (token) {
      this.socket = new WebSocket(urlConexao, ['jwt', token]);
    }
    return this.socket;
  }

  sendMessage(message: string) {
    this.socket.send(message);
  }
}
