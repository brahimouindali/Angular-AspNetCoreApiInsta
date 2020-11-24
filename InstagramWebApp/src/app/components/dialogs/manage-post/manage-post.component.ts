import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AccountService } from 'src/app/services/account.service';
import { Urls } from 'src/app/SETTINGS/URLS';
import { MedialistComponent } from '../../medialist/medialist.component';

@Component({
  selector: 'app-manage-post',
  templateUrl: './manage-post.component.html',
  styleUrls: ['./manage-post.component.css']
})
export class ManagePostComponent implements OnInit {

  value = `${Urls.WebAppUrl}/p/${this.data.id}`;
  constructor(
    public dialogRef: MatDialogRef<MedialistComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private accountService: AccountService,
    private _snackBar: MatSnackBar) { }

  ngOnInit(): void {
    // console.log(this.data)   
  }
  onUnfollow(id) {
    //console.log(id); 
    this.accountService.followOrUnfollow(id
      // my id 
    )
      .subscribe();
  }

  onModelClose() {
    this.dialogRef.close();
  }


  openSnackBar() {
    this.onModelClose();
    this._snackBar.open('Lien copié dans le presse-papiers.', '', {
      duration: 2000,
      panelClass: ['my-snack-bar']
    });
    let mysnackbar: any = document.querySelectorAll('.my-snack-bar')[0];
    mysnackbar.style.cssText += "color: #fff;position: relative;bottom: -1.5rem;width:100%";
    console.log(mysnackbar.style.cssText)
  }
}
