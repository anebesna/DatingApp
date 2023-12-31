import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {HomeComponent} from "./components/home/home.component";
import {MemberListComponent} from "./components/members/member-list/member-list.component";
import {MemberDetailComponent} from "./components/members/member-detail/member-detail.component";
import {ListsComponent} from "./components/lists/lists.component";
import {MessagesComponent} from "./components/messages/messages.component";
import {AuthGuard} from "./guards/auth.guard";
import {TestErrorComponent} from "./errors/test-error/test-error.component";
import {NotFoundComponent} from "./errors/not-found/not-found.component";
import {ServerErrorComponent} from "./errors/server-error/server-error.component";
import {MemberEditComponent} from "./components/members/member-edit/member-edit.component";
import {preventUnsavedChangesGuard} from "./guards/prevent-unsaved-changes.guard";
import {MemberDetailedResolver} from "./resolvers/member-detailed.resolver";
import {AdminPanelComponent} from "./components/admin/admin-panel/admin-panel.component";
import {AdminGuard} from "./guards/admin.guard";

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'members', component: MemberListComponent, canActivate: [AuthGuard]},
      {path: 'members/:username', component: MemberDetailComponent, resolve: {member: MemberDetailedResolver}},
      {path: 'member/edit', component: MemberEditComponent, canDeactivate: [preventUnsavedChangesGuard]},
      {path: 'lists', component: ListsComponent},
      {path: 'messages', component: MessagesComponent},
      {path: 'admin', component: AdminPanelComponent, canActivate: [AdminGuard]},
    ]
  },
  {path: 'errors', component: TestErrorComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', redirectTo: 'not-found', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
