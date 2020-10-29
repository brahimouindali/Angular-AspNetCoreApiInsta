import { Component, Inject, OnInit } from '@angular/core';
import { ProfileComponent } from '../profile/profile.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Media } from 'src/app/models/media';

@Component({
  selector: 'app-media-detail',
  templateUrl: './media-detail.component.html',
  styleUrls: ['./media-detail.component.css']
})
export class MediaDetailComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ProfileComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }


  ngOnInit(): void {  
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
