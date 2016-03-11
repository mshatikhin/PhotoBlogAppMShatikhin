using PhotoBlogApp.Core;
using PhotoBlogApp.Models.ClientModels;
using PhotoBlogApp.Models.Photos;

namespace PhotoBlogApp.Models.Service
{
    public interface IPhotoService
    {
        void SaveImages(FileUploadVm fileUploadVm, string rootPath);
        ViewDataUploadFileResult SaveImage(FileUploadM fileUploadVm, string rootPath);
        void DeleteImage(string login, int id, string rootPath);
        //List<ViewDataUploadFileResult> SaveImageFiles(FileUploadVm model);
        PhotosM BuildPhotos(GetPhotosParams photosParams);
        PhotoM BuildPhoto(Photo p);
    }
}