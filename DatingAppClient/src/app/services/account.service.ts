import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {BehaviorSubject, map} from "rxjs";
import {User} from "../models/user";
import {environment} from "../../environments/environment";
import {PresenceService} from "./presence.service";

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();
  constructor(private http: HttpClient, private presenceService: PresenceService) { }
  login(model: any){
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map(user => {
        if(user){
          this.setCurrentUser(user);
        }
      })
    )
  }
  register(model: any){
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if(user){
          this.setCurrentUser(user);
        }
      })
    )
  }
  setCurrentUser(user: User){
    user.roles = [];
    const roles = this.getDecodedToke(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
    this.presenceService.createHubConnection(user);
  }
  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presenceService.stopHubConnection();
  }
  getDecodedToke(token: string){
    return JSON.parse(atob(token.split('.')[1]));
  }

}
