import { Data } from "@angular/router";

export interface LikedBy {
    userId: number;
    postrName: string;
    postrId:string;
    postId: number
}

export interface Comment {
    id: number;
    comment: string;
    commentTime: Date;
    userId: number;
    userName: string;
    userPhoto: string;
    postId: number;
    postrId:number;
    postrName:string;

}

export interface Post {
    id: number;
    postTitle: string;
    postContent: string;
    postrName: string;
    postrPhoto: string;
    createdTime?: Data;
    photo?: string;
    likedBy?: Array<LikedBy>;
    comments?: Array<Comment>;
}


