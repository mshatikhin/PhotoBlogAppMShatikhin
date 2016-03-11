using System.Linq;

namespace PhotoBlogApp.Core.Models.Albums
{
    public interface IAlbumRepository
    {
        void Add(Album album);
        void Delete(int id);
        Album Find(int id);
        IQueryable<Album> GetAlbums(bool withHidden);
        void Save(Album album);
        Album FindByImageId(int id);
        Album GetAlbum(int id, bool withHidden);
    }
}
