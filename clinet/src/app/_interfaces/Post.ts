import { Data } from "@angular/router";

export interface Like {
    userId: number;
    postrName?: string;
    postrId?: string;
    postId?: number;
    userPhoto?: string;
    userName?: string
    likedTime?: Date;

}

export interface Comment {
    id: number;
    comment: string;
    commentTime: Date;
    userId: number;
    userName: string;
    userPhoto: string;
    postId: number;
    postrId: number;
    postrName: string;

}

export interface Post {
    id: number;
    postTitle: string;
    postContent: string;
    postrName: string;
    postrPhoto: string;
    postCategory: string;
    createdTime?: Data;
    photo?: string;
    likedBy?: Array<Like>;
    comments?: Array<Comment>;
}




