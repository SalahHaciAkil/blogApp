/// <reference types="@types/ckeditor" />
import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
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
  postCopy: Post;
  imageUploadFlag = false;

  // for image file
  publishFormData = new FormData();
  editFormData = new FormData();
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
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => { return false };
  }
  ngAfterViewInit(): void {
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.postId = +params.get("postId");
      if (this.postId != 0) {
        this.postService.getPost(this.postId).subscribe((post: Post) => {
          this.post = post;
          this.postCopy = Object.assign({}, this.post as Post);
          this.assignImageUrl()
        })
      }

    });
  }

  changedEditor(event: EditorChangeContent | EditorChangeSelection) {
  }


  publish() {

    this.setData(this.publishFormData)
    this.postService.addPost(this.publishFormData).subscribe((data) => {
      debugger;
      this.toastrService.success("Your post have been added successgully");
      this.router.navigateByUrl("/home");
    }, error => {
      console.log("error", error.error);

      // this.toastrService.info("You can't create a post if you are not registered");
    })
  }

  setData(formData) {
    formData.set('postTitle', this.post.postTitle);
    formData.set('postContent', this.post.postContent);
    formData.set('postCategory', this.post.postCategory);
  }

  edit() {
    this.setData(this.editFormData)
    this.editFormData.set("id", this.post.id.toString());

    this.postService.editPost(this.editFormData).subscribe((photo: string) => {
      console.log(photo);
      this.toastrService.success("post edited successfully", "Succeess");

    }, error => {
      this.post = this.postCopy;
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
    if (this.postId == 0)
      this.publishFormData.set('photo', fileToUpload, fileToUpload.name);
    else
      this.editFormData.set('photo', fileToUpload, fileToUpload.name);


    let reader = new FileReader();
    this.imagePath = files;
    reader.readAsDataURL(files[0]);
    reader.onload = (_event) => {
      this.imgURL = reader.result;
    }

    this.imageUploadFlag = true;
  }


  private assignImageUrl() {
    this.imgURL = this.post.photo;
    this.imageUploadFlag = true;

  }

}
