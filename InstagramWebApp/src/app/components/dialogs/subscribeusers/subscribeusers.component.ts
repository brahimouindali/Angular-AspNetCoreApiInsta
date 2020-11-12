import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AccountService } from 'src/app/services/account.service';
import { MedialistComponent } from '../../medialist/medialist.component';

@Component({
  selector: 'app-subscribeusers',
  templateUrl: './subscribeusers.component.html',
  styleUrls: ['./subscribeusers.component.css']
})
export class SubscribeusersComponent implements OnInit {

  userImagePath = 'https://localhost:44398/profile';
  // photo: string = '';

  constructor(
    public dialogRef: MatDialogRef<MedialistComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private accountService: AccountService) { }

  ngOnInit(): void {
    console.log(this.data)
  }

  getImgePath(url) {
    if (url == null)
      return '../../../assets/no-img.png';
    return `${this.userImagePath}/${url}`;
  }

  onFollowOrUnfollow(id, index) {
    this.accountService.followOrUnfollow(id).subscribe(res => {
      console.log(res)
      this.data[index].iFollowedUser = !this.data[index].iFollowedUser;
      this. onModelClose();
    },
      err => { 
        this.data[index].iFollowedUser = this.data[index].iFollowedUser;
        console.log(err.error);
      });
   
  }

  onModelClose() {
    this.dialogRef.close()
  }

}
