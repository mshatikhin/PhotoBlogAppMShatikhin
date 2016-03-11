

module PhotoBlogApp.Api
{
    export class DataLoader
    {
        load: (data: any, success: (result: any) => void) => void;
        constructor(url: string, httpMethod: string)
        {
            this.load = (data, success) =>
            {
                var jsonData = data == null ? null : ko.mapping.toJSON(data);
                var settings: JQueryAjaxSettings = {
                    url: url,
                    dataType: "json",
                    type: httpMethod,
                    data: jsonData,
                    contentType: "application/json; charset=utf-8",
                    error: () => { alert('При запросе данных произошла ошибка'); },
                    beforeSend: () => {
                        jQuery("#photos-loader").show();
                        jQuery("#loader-image").show();
                    },
                    success: data => {
                        jQuery("#loader-image").hide();
                        success(data);
                    },
                    complete: () => {
                        jQuery("#photos-loader").hide();
                    }
                };
                jQuery.ajax(settings);
            };
        }
    }

    export class AsyncClient
    {
        static Current = new AsyncClient();

        GetPhotos(getPhotosParams: GetPhotosParams, success: (result: PhotosM) => void) {
            var dataLoader = new DataLoader("/getPhotos","Post");
            dataLoader.load(getPhotosParams, (result) =>
            {
                success(result);
            });
        }

        GetPosts(success: (result: PostsM) => void) {
            var dataLoader = new DataLoader("/Home/getPosts", "Post");
            dataLoader.load(null,(result) =>
            {
                success(result);
            });
        }

        GetAlbums(success: (result: AlbumsM) => void) {
            var dataLoader = new DataLoader("/Home/getAlbums", "Post");
            dataLoader.load(null, (result) =>
            {
                success(result);
            });
        }

        GetPhotoProjects(success: (result: PhotoProjectM[]) => void) {
            var dataLoader = new DataLoader("/Home/getPhotoProjects", "Post");
            dataLoader.load(null,(result) =>
            {
                success(result);
            });
        }
    }
 }