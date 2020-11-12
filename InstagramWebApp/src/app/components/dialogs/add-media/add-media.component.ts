import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MediaService } from 'src/app/services/media.service';

@Component({
  selector: 'add-media',
  templateUrl: './add-media.component.html',
  styleUrls: ['./add-media.component.css']
})
export class AddMediaComponent implements OnInit {

  @ViewChild('fileInput') fileInput: ElementRef;
  newMediaForm: FormGroup;
  file = null;

  constructor(private mediaService: MediaService) { }

  ngOnInit(): void {
    this.initForm();
  }
  initForm() {
    this.newMediaForm = new FormGroup({
      File: new FormControl(null, Validators.required),
      Description: new FormControl(null, [Validators.required, Validators.minLength(5)])
    });
  }

  onFileUpload() {
    var nativeElement: HTMLInputElement = this.fileInput.nativeElement
    this.file = nativeElement.files[0] 
  }

  onSubmit(data) {
    var formMedia = new FormData();
    formMedia.append('File', this.file);
    formMedia.append('Description', data.Description);

    this.mediaService.addMedia(formMedia)
      .subscribe(result => {
        console.log(result);
        this.newMediaForm.reset();
      }, error => {
        console.error(error);
      });
  }
}
