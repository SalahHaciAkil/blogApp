/// <reference types="@types/ckeditor" />
import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
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
  //Editing post variables
  postId: number = -1;
  post: Partial<Post> = { postTitle: "", postContent: "", postCategory: "" };
  imageChanged: string = "unchanged"
  imageUploadFlag = false;

  // for image file
  formData = new FormData();
  public imagePath;
  imgURL: any;
  public message: string;


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

      ['link']                         // link and image, video
    ]
  };



  constructor(
    private postService: PostService,
    private toastrService: ToastrService,
    private route: ActivatedRoute
  ) {

  }
  ngAfterViewInit(): void {
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.postId = +params.get("postId");
      if (this.postId != 0) {
        this.postService.getPost(this.postId).subscribe((post: Post) => {
          this.post = post;
          this.assignImageUrl()
        })
      }

    });
  }

  changedEditor(event: EditorChangeContent | EditorChangeSelection) {
  }


  publish() {
    this.formData.set('postTitle', this.post.postTitle);
    this.formData.set('postContent', this.post.postContent);
    this.formData.set('postCategory', this.post.postCategory);

    this.toastrService.success(String(this.formData.get("postCategory")));

    this.postService.addPost(this.formData).subscribe((data) => {
      this.toastrService.success("Your post have been added successgully");
    }, error => {
      this.toastrService.info("You can't create a post if you are not registered");
    })
  }

  edit() {
    console.log(this.imageChanged, this.post);

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
    this.formData.set('photo', fileToUpload, fileToUpload.name);

    let reader = new FileReader();
    this.imagePath = files;
    reader.readAsDataURL(files[0]);
    reader.onload = (_event) => {
      this.imgURL = reader.result;
    }

    this.imageUploadFlag = true;
    this.imageChanged = "changed" // in case of editing the post
  }


  private assignImageUrl() {
    this.imgURL = this.post.photo;
    this.imageUploadFlag = true;

  }

}
