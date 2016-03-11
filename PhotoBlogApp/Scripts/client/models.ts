module PhotoBlogApp.Api
{
    export class PhotoProjectM
    {
        PhotoProjectId: number;
        Name: string;
        Description: string;
    }

    export class AlbumM
    {
        Year: number;
        Name: string;
        Logo: string;
        Description: string;
        Photos: PhotoM[];
    }

    export class AlbumsM
    {
        AlbumMs: AlbumM[];
    }

    export class PostM
    {
        PostId: number;
        Title: string;
        HTML: string;
        Author: string;
    }

    export class PostsM
    {
        Posts: PostM[];
    }

    export class PhotoM
    {
        PhotoId: number;
        Url: string;
        FullUrl: string;
        Title: string;
        AlbumName: string;
        AlbumDescription: string;
    }

    export class GetPhotosParams
    {
        Skip: number;
        Take: number;
        AlbumId: number;
        NumberDay: number;
    }

    export class PhotosM
    {
        Skip: number;
        Scrolled: boolean;
        Photos: PhotoM[];
    }
} 