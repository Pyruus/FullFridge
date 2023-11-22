export interface Posts {
    posts: Post[];
    pages: number;
}

export interface Post {
    id: string,
    title: string,
    content: string,
    createdBy: string,
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