import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Location, LocationStrategy, PathLocationStrategy } from '@angular/common';

import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { RegisterComponent } from './register/register.component';
import { RegisterService } from './_services/register.service';
import { ChatComponent } from './chat/chat.component';
import { ChatService } from './_services/chat.service';

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      RegisterComponent,
      ChatComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule
   ],
   providers: [
      AuthService,
      RegisterService,
      ChatService,
      Location,
      {provide: LocationStrategy, useClass: PathLocationStrategy}
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
