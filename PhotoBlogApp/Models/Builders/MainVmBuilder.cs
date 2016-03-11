using System;
using System.Linq;
using PhotoBlogApp.Core;
using PhotoBlogApp.Core.Models.Albums;
using PhotoBlogApp.Core.Models.Photos;
using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Builders
{
    public class MainVmBuilder : IMainVmBuilder
    {
        private readonly IPhotoRepository photoRepository;
        private readonly IAlbumRepository albumRepository;
        private const string ThumbnailFolder = "thumb";
        private const string SmallImageFolder = "s";
        private const string MediumImageFolder = "m";


        public MainVmBuilder(IPhotoRepository photoRepository, IAlbumRepository albumRepository)
        {
            this.photoRepository = photoRepository;
            this.albumRepository = albumRepository;
        }

        public MainViewM BuildPhotos(GetPhotosParams getPhotosParams)
        {
            var photos = photoRepository
                .GetPhotosByAlbumId(getPhotosParams.AlbumId)
                .OrderByDescending(p => p.Date)
                .Skip(getPhotosParams.Skip)
                .Take(getPhotosParams.Take)
                .ToArray();
            return BuildPhotos(getPhotosParams, photos);
        }

        private static MainViewM BuildPhotos(GetPhotosParams getPhotosParams, Photo[] photos)
        {
            var model = new MainViewM
            {
                PhotoM = photos
                    .Select((p, index) =>
                        new PhotoM
                        {
                            PhotoId = p.PhotoId,                            
                            Title = p.PhotoName,
                            FullUrl = PathBuild(p),
                            Url = ThumbnailPathBuild(p)                            
                        }).ToArray()
            };
            model.Skip = getPhotosParams.Skip + model.PhotoM.Length;
            model.Scrolled = photos.Any();
            return model;
        }

        public AlbumsViewM BuildAlbums()
        {
            var albums = albumRepository.GetAlbums(withHidden: false);
            if (albums.Any())
            {
                var model = new AlbumsViewM
                {
                    AlbumMs = albums
                        .ToArray()
                        .Where(a => a.Photos.Any())
                        .Select(a =>
                        {
                            var photo = a.Photos.FirstOrDefault();

                            var logo = photo != null ? ThumbnailPathBuild(photo) : "Content/images/preview.png";
                            return new AlbumM
                            {
                                AlbumId = a.AlbumId,
                                Description = a.Description,
                                Logo = logo,
                                Name = a.AlbumName
                            };
                        })
                        .ToArray()
                };
                return model;
            }
            return new AlbumsViewM();
        }

        public AlbumPhotosViewM BuildPhotoAlbums(int id)
        {
            var album = albumRepository.GetAlbum(id, withHidden: false);
            var albums = albumRepository.GetAlbums(withHidden: false);
            var photos = album.Photos.ToArray();

            var model = new AlbumPhotosViewM
            {
                Albums = albums
                    .Where(a => a.Photos.Any())
                    .Select(a => new AlbumM
                    {
                        AlbumId = a.AlbumId,
                        Name = a.AlbumName
                    })
                    .ToArray(),
                Album = new AlbumM
                {
                    AlbumId = album.AlbumId,
                    Description = album.Description,
                    Name = album.AlbumName
                },
                Photos = photos.Select((p, index) => new PhotoM
                {
                    PhotoId =  p.PhotoId,
                    FullUrl = PathBuild(p),
                    Url = MediumPathBuild(p),
                    SmallUrl = SmallPathBuild(p),
                    ThumbUrl = ThumbnailPathBuild(p),
                    Title = p.PhotoName,
                }).ToArray()
            };
            return model;
        }

        public bool SavePhoto(PhotoM photo)
        {
            var oldPhoto = photoRepository.Find(photo.PhotoId);            
            try
            {
                photoRepository.Save(oldPhoto);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public PortfolioViewModel BuildPortfolio()
        {
            var albumId = 1;//365 лиц
            var album = albumRepository.GetAlbum(albumId, withHidden: false);
            var photos = album.Photos.OrderByDescending(p => p.Date).ToArray();

            var model = new PortfolioViewModel
            {
                PortfolioPhoto =
                    photos.Select(p => new PortfolioPhotoM
                    {
                        Day = p.Date.DayOfYear.ToString(),
                        ImageUrl = PathBuild(p),
                        ImageThumbUrl = ThumbnailPathBuild(p)
                    }).ToArray()
            };

            return model;
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