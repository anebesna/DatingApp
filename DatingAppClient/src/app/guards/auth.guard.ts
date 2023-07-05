import { Injectable } from '@angular/core';
import {CanActivate} from '@angular/router';
import {map, Observable} from 'rxjs';
import {AccountService} from "../services/account.service";
import {ToastrService} from "ngx-toastr";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountService: AccountService, private toastr: ToastrService) {
  }
  canActivate(): Observable<boolean>  {
    return this.accountService.currentUser$.pipe(
      map(user => {
          if (user) return true;
          else {
            this.toastr.error('You do not have access to this page.');
            return false;
          }
        }
      )
    )
  }

}
