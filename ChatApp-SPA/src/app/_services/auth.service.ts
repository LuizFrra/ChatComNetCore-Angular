import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl: string;

constructor(private http: HttpClient) {
  this.baseUrl = environment.apiUrl + ':' + environment.port;
}

login(model: any) {
  return this.http.post(this.baseUrl + '/api/auth/login', model)
    .pipe(
      map((response: any) => {
        if (response) {
          localStorage.setItem('token', response.token);
          localStorage.setItem('username', response.name);
          // this.http.get(this.baseUrl + '/api/auth/generateGuid').subscribe(
          //   next => console.log(next)
          // );
        }
      })
    );
}
}
