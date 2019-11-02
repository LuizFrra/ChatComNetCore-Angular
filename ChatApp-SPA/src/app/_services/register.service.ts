import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {
  baseUrl: string;
constructor(private httpClient: HttpClient) {
  this.baseUrl = environment.apiUrl + ':' + environment.port + '/api/auth/';
}

register(model: any) {
  return this.httpClient.post(this.baseUrl + 'register', model)
    .pipe(
      map((response: any) => {
        if (response) {
          console.log(response);
          localStorage.setItem('username', response.name);
        }
      })
    );
}

}
