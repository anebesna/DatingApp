import {Component, OnInit} from '@angular/core';
import {AccountService} from "./services/account.service";
import {User} from "./models/user";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'DatingAppClient';
  constructor(private accountService: AccountService) {
  }
  ngOnInit() {
    this.setCurrentUser();
  }
  setCurrentUser(){
    const user: User = JSON.parse(localStorage.getItem('user')!);
    if (!user) return;
    this.accountService.setCurrentUser(user);
  }
}
