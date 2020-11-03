import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/models/user';
import { CommentService } from 'src/app/services/comment.service';

@Component({
  selector: 'app-commentform',
  templateUrl: './commentform.component.html',
  styleUrls: ['./commentform.component.css']
})
export class CommentformComponent implements OnInit {

  @Input() mediaId: number;
  @Input() user: User;
  @Output() notify: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    private fb: FormBuilder,
    private commentService: CommentService
  ) { }

  ngOnInit(): void { }

  newCommentForm: FormGroup = this.fb.group({
    'Comment': [null, Validators.required]
  });

  onSubmit(data) {
    let comment = {
      appUser: this.user,
      mediaId: this.mediaId,
      content: data.Comment,
      publishedAt: new Date()
    };
    this.commentService.addComment(comment)
      .subscribe(() => {
        this.notify.emit(comment)
        this.newCommentForm.reset()
      },
        err => {
          console.log(err);
        }
      )
  }

}
