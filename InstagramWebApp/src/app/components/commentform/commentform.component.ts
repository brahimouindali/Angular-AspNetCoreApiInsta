import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CommentService } from 'src/app/services/comment.service';

@Component({
  selector: 'app-commentform',
  templateUrl: './commentform.component.html',
  styleUrls: ['./commentform.component.css']
})
export class CommentformComponent implements OnInit {

  @Input() mediaId: number;

  constructor(
    private fb: FormBuilder,
    private commentService: CommentService
  ) { }

  ngOnInit(): void {
  }

  newCommentForm: FormGroup = this.fb.group({
    'Comment': [null, Validators.required]
  });

  onSubmit(data) {
    let comment = {
      mediaId: this.mediaId,
      content: data.Comment
    };
    this.commentService.addComment(comment)
      .subscribe(result => {
        this.newCommentForm.reset()
        console.log(result);
      }
      )
  }

}
