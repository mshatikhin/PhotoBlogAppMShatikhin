using System.Linq;
using PhotoBlogApp.Core;
using PhotoBlogApp.Core.Models.Albums;
using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Service
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository albumRepository;
        private const string ThumbnailFolder = "thumb";
        private const string SmallImageFolder = "s";
        private const string MediumImageFolder = "m";

        public AlbumService(IAlbumRepository albumRepository)
        {
            this.albumRepository = albumRepository;
        }

        public AlbumM[] GetAlbums()
        {
            var albums = albumRepository
                    .GetAlbums(false)
                    .OrderByDescending(a => a.Order)
                    .ToList()
                    .Select(BuildAlbumM)
                    .ToArray();
            return albums;
        }

        public AlbumM BuildAlbumM(Album album)
        {
            return new AlbumM
            {
                AlbumId = album.AlbumId,
                Name = album.AlbumName,
                TitleUrl = album.TranslitName,
                Logo = album.Logo,
                Description = album.Description,
                Photos = album.Photos.Select(p => new PhotoM
                {
                    PhotoId = p.PhotoId,
                    ThumbUrl = ThumbnailPathBuild(p),
                    SmallUrl = SmallPathBuild(p),
                    Url = MediumPathBuild(p),
                    FullUrl = PathBuild(p),
                    Title = p.PhotoName,
                    AlbumName = album.AlbumName,
                    AlbumDescription = album.Description,
                    IsWide = p.Wide
                })
                .OrderByDescending(p => p.PhotoId)
                .ToArray()
            };
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