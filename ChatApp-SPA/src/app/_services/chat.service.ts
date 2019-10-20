import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Location } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
socket: WebSocket;

constructor(private http: HttpClient, location: Location) { }

connectToServer() {
  const protocolo = location.protocol === 'https' ? 'wss://' : 'ws://';
  const porta = location.port ? (':' + location.port) : '';
  const urlConexao = protocolo + location.hostname + porta + '/Chat';
  //this.socket = new WebSocket(urlConexao);
  console.log(urlConexao);
}

}
