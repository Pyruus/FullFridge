export interface Posts {
    posts: Post[];
    pages: number;
}

export interface Post {
    id: string,
    title: string,
    content: string,
    createdBy: User,
    createdAt: string,
    recipeId: string
}

export interface Comment{
    id: string,
    content: string,
    name: string,
    surname: string,
    createdAt: string
}

export interface NewComment {
    content: string,
    postId: string,
    createdBy: string
}

export interface User {
    id: string,
    email: string,
    name: string,
    surname: string
}