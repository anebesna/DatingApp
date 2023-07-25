import { Component, OnInit } from '@angular/core';
import {User} from "../../../models/user";
import {AdminService} from "../../../services/admin.service";
import {BsModalRef, BsModalService, ModalOptions} from "ngx-bootstrap/modal";
import {RoleModalComponent} from "../../modals/role-modal/role-modal.component";
import {initialState} from "ngx-bootstrap/timepicker/reducer/timepicker.reducer";

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {

  users: User[] = [];
  bsModalRef: BsModalRef<RoleModalComponent> = new BsModalRef<RoleModalComponent>();
  availableRoles = [
    'Admin',
    'Moderator',
    'Member'
  ]
  constructor(private adminService: AdminService, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  getUsersWithRoles(){
    this.adminService.getUsersWithRoles().subscribe({
      next: users => this.users = users
    })
  }
  openRolesModal(user: User){
    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        username: user.username,
        availableRoles: this.availableRoles,
        selectedRoles: [...user.roles]
      }
    }
    this.bsModalRef =  this.modalService.show(RoleModalComponent, config);
    this.bsModalRef.onHide?.subscribe({
      next: () => {
        const selectedRoles = this.bsModalRef.content?.selectedRoles;
        if (!this.arrayEqual(selectedRoles!, user.roles)){
          this.adminService.updateUserRoles(user.username, selectedRoles!).subscribe({
            next: roles => user.roles = roles
          })
        }
      }
    })
  }
  private arrayEqual(arr1: any[], arr2: any[]){
    return JSON.stringify(arr1.sort()) === JSON.stringify(arr2.sort());
  }
}
