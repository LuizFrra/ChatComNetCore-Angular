import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {
  baseUrl = 'http://localhost:5000/api/auth/';
constructor(private httpClient: HttpClient) { }

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
