/// <reference types="@types/ckeditor" />
import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { CKEditorComponent } from 'ng2-ckeditor';
import { EditorChangeContent, EditorChangeSelection } from 'ngx-quill';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.scss']
})
export class CreatePostComponent implements OnInit, AfterViewInit {
  postContent: string;
  postTitle: string;

  @ViewChild("paragra") q: ElementRef;
  editorModules = {
    toolbar: [
      ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
      ['blockquote', 'code-block'],

      [{ 'header': 1 }, { 'header': 2 }],               // custom button values
      [{ 'list': 'ordered' }, { 'list': 'bullet' }],
      [{ 'script': 'sub' }, { 'script': 'super' }],      // superscript/subscript
      [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
      [{ 'direction': 'rtl' }],                         // text direction

      [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
      [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

      [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
      [{ 'font': [] }],
      [{ 'align': [] }],

      ['clean'],                                         // remove formatting button

      ['link', 'image', 'video']                         // link and image, video
    ]
  };


  constructor() {
    this.postContent = `<p>Write...</p>`;
  }
  ngAfterViewInit(): void {
    this.q.nativeElement.innerHTML = this.postContent;
  }

  ngOnInit() {

  }
  changedEditor(event: EditorChangeContent | EditorChangeSelection) {
  }
  onChange(event) {
    console.log(event);

  }

  insert() {
    console.log(this.postContent);
    console.log(this.postTitle);
    

  }

}
