import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AccountService } from 'src/app/services/account.service';
import { MedialistComponent } from '../medialist/medialist.component';

@Component({
  selector: 'app-subscribeusers',
  templateUrl: './subscribeusers.component.html',
  styleUrls: ['./subscribeusers.component.css']
})
export class SubscribeusersComponent implements OnInit {

  userImagePath = 'https://localhost:44398/profile';

  constructor(
    public dialogRef: MatDialogRef<MedialistComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private accountService: AccountService) { }

  ngOnInit(): void {
  }

  onFollowOrUnfollow(id, index) {
    this.accountService.followOrUnfollow(id).subscribe(() => {
      this.data[index].isFollowedMe = !this.data[index].isFollowedMe;
    },
    err => {
      this.data[index].isFollowedMe = this.data[index].isFollowedMe;
    });

    // if (!this.data[index].isFollowedMe) {
    //   this.data[index].isFollowedMe = true;
    // }
    // else {
    //   this.data[index].isFollowedMe = false;
    // }    
  }

}
