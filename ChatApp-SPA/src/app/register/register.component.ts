import { Component, OnInit } from '@angular/core';
import { RegisterService } from '../_services/register.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {
  model: any = { };

  constructor(private registerService: RegisterService) { }

  ngOnInit() {
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token;
  }

  register() {
    console.log(this.model);
    this.registerService.register(this.model).subscribe(next => {
      console.log('Registered Succefuly');
    }, error => {
      console.log('Error When Registered');
    });
  }

}
