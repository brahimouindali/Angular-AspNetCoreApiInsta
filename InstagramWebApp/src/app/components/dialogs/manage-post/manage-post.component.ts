import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AccountService } from 'src/app/services/account.service';
import { MedialistComponent } from '../../medialist/medialist.component';

@Component({
  selector: 'app-manage-post',
  templateUrl: './manage-post.component.html',
  styleUrls: ['./manage-post.component.css']
})
export class ManagePostComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<MedialistComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private accountService: AccountService) { }

  ngOnInit(): void {
        
  }
  onUnfollow(id) {
    console.log(id); 
    this.accountService.followOrUnfollow(id
      // my id 
      )
   .subscribe();
  }

  onModelClose() {
    this.dialogRef.close();
  }
}
