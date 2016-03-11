using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using PhotoBlogApp.Core;
using PhotoBlogApp.Core.Helpers;
using PhotoBlogApp.Core.Models.Albums;
using PhotoBlogApp.Core.Models.Photos;
using PhotoBlogApp.Models.ClientModels;
using PhotoBlogApp.Models.Photos;

namespace PhotoBlogApp.Models.Service
{
    public class PhotoService : IPhotoService
    {
        private static readonly string PhotosVirtualPath = ConfigurationManager.AppSettings["PhotosVirtualPath"];
        private readonly IPhotoRepository photoRepository;
        private readonly IAlbumRepository albumRepository;
        private const string ThumbnailFolder = "thumb";
        private const string SmallImageFolder = "s";
        private const string MediumImageFolder = "m";

        public PhotoService(IPhotoRepository photoRepository, IAlbumRepository albumRepository)
        {
            this.photoRepository = photoRepository;
            this.albumRepository = albumRepository;
        }

        public PhotosM BuildPhotos(GetPhotosParams photosParams)
        {
            var photosFromDb = photoRepository.GetPhotosByAlbumId(photosParams.AlbumId).Select(p => p).ToArray();
            var photoMs = photosFromDb
                .Select(BuildPhoto)
                .ToArray();
            var photos = photoMs
            .Skip(photosParams.Skip)
            .Take(photosParams.Take)
            .ToArray();

            return new PhotosM
            {
                Skip = photos.Length,
                Photos = photos,
                Scrolled = photos.Length > 0
            };
        }

        public PhotoM BuildPhoto(Photo p)
        {
            return new PhotoM
            {
                FullUrl = PathBuild(p),
                Url = MediumPathBuild(p),
                SmallUrl = SmallPathBuild(p),
                ThumbUrl = ThumbnailPathBuild(p),                
                Title = p.PhotoName,
                AlbumName = p.Album.AlbumName,
                IsWide = p.Wide
            };
        }

        public void SaveImages(FileUploadVm fileUploadVm, string rootPath)
        {
            foreach (var file in fileUploadVm.UploadedFiles.Where(file => file != null))
            {
                SaveImageFile(file, fileUploadVm.Login, fileUploadVm.AlbumId, rootPath);
            }
            var httpFileCollectionBase = fileUploadVm.UploadedRequestFiles;
            if (httpFileCollectionBase == null) return;
            for (var i = 0; i < httpFileCollectionBase.Count; i++)
            {
                SaveImageFile(httpFileCollectionBase[i], fileUploadVm.Login, fileUploadVm.AlbumId, rootPath);
            }
        }

        public ViewDataUploadFileResult SaveImage(FileUploadM fileUploadM, string rootPath)
        {
            if (fileUploadM.UploadedFile != null)
            {
                return SaveImageFile(fileUploadM.UploadedFile, fileUploadM.Login, fileUploadM.AlbumId, rootPath);
            }
            return null;
        }

        public void DeleteImage(string login, int id, string rootPath)
        {
            var album = albumRepository.FindByImageId(id);
            if (album != null)
            {
                var pathDisc = BuildPath(login, string.Format("album-{0}", album.AlbumId), id, rootPath);
                if (Directory.Exists(pathDisc))
                {
                    Directory.Delete(pathDisc, recursive: true);
                }
                photoRepository.Delete(id);
            }
        }

        public List<ViewDataUploadFileResult> SaveImageFiles(FileUploadVm model, string rootPath)
        {
            var statuses = new List<ViewDataUploadFileResult>();
            for (var i = 0; i < model.UploadedRequestFiles.Count; i++)
            {
                var st = SaveImageFile(model.UploadedRequestFiles[i], model.Login, model.AlbumId, rootPath);
                statuses.Add(st);
            }
            return statuses;
        }


        private Photo SaveImageDb(HttpPostedFileBase file, int albumId, string webPath, string fileName)
        {
            var photo = new Photo
            {
                AlbumId = albumId,
                Path = webPath.Replace('\\', '/'),
                Extention = file.ContentType,
                PhotoName = fileName,
                Date = DateTime.Now,
                Description = ""                
            };
            return photoRepository.Add(photo);
        }

        private static string BuildPath(string login, string albumPath, int imgId, string rootPath)
        {
            var fullPath = Path.Combine(rootPath, login, albumPath, imgId.ToString()).Replace('\\', '/');
            return fullPath;
        }

        private static string BuildWebPath(string login, string albumPath)
        {            
            return string.Format("{0}{1}/{2}/", PhotosVirtualPath, login, albumPath);
        }

        public static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public ViewDataUploadFileResult SaveImageFile(HttpPostedFileBase file, string login, int albumId, string rootPath)
        {
            var checkContentType = CheckContentType(file);
            if (checkContentType)
            {
                var filename = Path.GetFileName(file.FileName);
                if (filename != null)
                {
                    var albumPath = string.Format("album-{0}", albumId);
                    var webPath = BuildWebPath(login, albumPath);
                    filename = string.Format("{0}{1}", Guid.NewGuid(), Path.GetExtension(filename));
                    var savedPhoto = SaveImageDb(file, albumId, webPath, filename);
                    if (savedPhoto != null)
                    {
                        var pathDisc = BuildPath(login, albumPath, savedPhoto.PhotoId, rootPath);
                        
                        SavePhotoInFolder(file, pathDisc + "/thumb", filename, 96, 64);
                        SavePhotoInFolder(file, pathDisc + "/s", filename, 960, 320);
                        SavePhotoInFolder(file, pathDisc + "/m", filename, 960, 640);
                        var imageProps = SavePhotoInFolder(file, pathDisc, filename, 2560, 1600);
                        if (imageProps != null)
                        {
                            SaveImagePropsToDb(savedPhoto.PhotoId, imageProps);
                        }
                        var thumbnailUrl = string.Format("{0}{1}/s/{2}", webPath, savedPhoto.PhotoId, filename);
                        return BuildUploadFileResult(file, thumbnailUrl);
                    };
                }
            }
            return new ViewDataUploadFileResult
            {
                Error = "Файл должен быть типа: jpeg, png, gif"
            };
        }

        private void SaveImagePropsToDb(int photoId, ImageProps imageProps)
        {
            var photo = photoRepository.Find(photoId);
            if (photo != null)
            {
                photo.Wide = imageProps.W > imageProps.H;
            }
            photoRepository.Save(photo);
        }

        private static bool CheckContentType(HttpPostedFileBase file)
        {
            return file.ContentType.Equals("image/jpeg") ||
                   file.ContentType.Equals("image/png");
        }

        private static ImageProps SavePhotoInFolder(HttpPostedFileBase file, string pathDisc, string filename, int fW, int fH)
        {
            CreateDirectory(pathDisc);
            if (Directory.Exists(pathDisc))
            {
                try
                {
                    var imageProps = ImageHelper.BuildImage(file, pathDisc + '/', filename, fW, fH);
                    return imageProps;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return null;
        }

        private ViewDataUploadFileResult BuildUploadFileResult(HttpPostedFileBase file, string thumbnailUrl)
        {
            var st = new ViewDataUploadFileResult
            {
                Title = file.FileName,
                ThumbnailUrl = thumbnailUrl
            };
            return st;
        }

        private static string ThumbnailPathBuild(Photo photo)
        {
            return string.Format("{0}/{1}/{2}", photo.Path, ThumbnailFolder, photo.PhotoName);
        }

        private static string MediumPathBuild(Photo photo)
        {
            return string.Format("{0}/{1}/{2}", photo.Path, MediumImageFolder, photo.PhotoName);
        }

        private static string SmallPathBuild(Photo photo)
        {
            return string.Format("{0}/{1}/{2}", photo.Path, SmallImageFolder, photo.PhotoName);
        }

        private static string PathBuild(Photo photo)
        {
            return string.Format("{0}/{1}", photo.Path, photo.PhotoName);
        }
    }
}