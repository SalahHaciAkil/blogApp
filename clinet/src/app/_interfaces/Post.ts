import { Data } from "@angular/router";

export interface LikedBy {
    userId: number;
    postrName: string;
    postId: number
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
}


