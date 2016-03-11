using System.Data.Entity;
using System.Linq;

namespace PhotoBlogApp.Core.Models.Albums
{
    public class AlbumRepository : IAlbumRepository
    {
        public void Add(Album album)
        {
            using (var db = new DbModelContainer())
            {
                db.Albums.Add(album);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new DbModelContainer())
            {
                var album = db.Albums.Find(id);
                db.Entry(album).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public Album Find(int id)
        {
            using (var db = new DbModelContainer())
            {
                return db.Albums.Include(x => x.Photos).FirstOrDefault(p => p.AlbumId == id);
            }
        }

        public IQueryable<Album> GetAlbums(bool withHidden)
        {
            var db = new DbModelContainer();
            if (withHidden)
            {
                return db.Albums.Include(x => x.Photos);
            }
            return db.Albums.Include(x => x.Photos).Where(a => !a.Hide);
        }

        public void Save(Album album)
        {
            using (var db = new DbModelContainer())
            {
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public Album FindByImageId(int id)
        {
            using (var db = new DbModelContainer())
            {
                var album = db.Albums.FirstOrDefault(a => a.Photos.Any(p => p.PhotoId == id));
                if (album != null)
                {
                    return album;
                }
                return null;
            }
        }

        public Album GetAlbum(int id, bool withHidden)
        {
            using (var db = new DbModelContainer())
            {
                if (withHidden)
                {
                    return db.Albums.Include(x=>x.Photos).SingleOrDefault(a => a.AlbumId == id);
                }
                return db.Albums.Include(x => x.Photos).SingleOrDefault(a => a.AlbumId == id && !a.Hide);
            }
        }
    }
}