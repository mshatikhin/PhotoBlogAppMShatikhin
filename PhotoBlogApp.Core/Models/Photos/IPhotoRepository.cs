using System.Linq;

namespace PhotoBlogApp.Core.Models.Photos
{
    public interface IPhotoRepository
    {
        Photo Add(Photo photo);
        void Delete(int id);
        Photo Find(int id);
        IQueryable<Photo> GetPhotos();
        void Save(Photo photo);
        IQueryable<Photo> GetPhotosByAlbumId(int id);
    }
}
