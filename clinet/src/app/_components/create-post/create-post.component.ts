/// <reference types="@types/ckeditor" />
import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { CKEditorComponent } from 'ng2-ckeditor';
import { EditorChangeContent, EditorChangeSelection } from 'ngx-quill';
import { ToastrService } from 'ngx-toastr';
import { Post } from 'src/app/_interfaces/Post';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.scss']
})
export class CreatePostComponent implements OnInit, AfterViewInit {
  post: Post = { postTitle: "", postContent: "", postrName: "", postrPhoto:"", id: 0 }
  // post:PaPost;
  imageUploadFlag = false;

  // for image file
  formData = new FormData();
  public imagePath;
  imgURL: any;
  public message: string;

  //  postContent:string;
  //   @ViewChild("paragra") q: ElementRef;
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



  constructor(private postService: PostService, private toastrService: ToastrService) {
    // this.post.postContent=""
    // this.post.postTitle=""
    // this.post.postContent = `<p>Write...</p>`;
  }
  ngAfterViewInit(): void {
    // this.q.nativeElement.innerHTML = this.post.postContent;
  }

  ngOnInit() {

  }
  changedEditor(event: EditorChangeContent | EditorChangeSelection) {
  }
  // onChange(event) {
  //   console.log(event);

  // }


  publish() {

    this.formData.append('postTitle', this.post.postTitle);
    this.formData.append('postContent', this.post.postContent);


    this.postService.addPost(this.formData).subscribe((data) => {
      debugger;

      this.toastrService.success("Your post have been added successgully");
    })
  }


  preview(files) {
    if (files.length === 0)
      return;


    let mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      this.message = "Only images are supported.";
      return;
    }

    //append the image file
    let fileToUpload = <File>files[0];
    this.formData.append('photo', fileToUpload, fileToUpload.name);

    let reader = new FileReader();
    this.imagePath = files;
    reader.readAsDataURL(files[0]);
    reader.onload = (_event) => {
      this.imgURL = reader.result;
    }

    this.imageUploadFlag = true;
  }

}
