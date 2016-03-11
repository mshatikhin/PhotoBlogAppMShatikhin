using System;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace PhotoBlogApp.Core.Models.Photos
{
    public class PhotoRepository : IPhotoRepository
    {
        public Photo Add(Photo photo)
        {
            var db = new DbModelContainer();
            try
            {
                var exist = db.Photos.Any(p => p.PhotoName == photo.PhotoName);
                if (!exist)
                {
                    db.Photos.Add(photo);
                    db.SaveChanges();

                    photo.Path = Path.Combine(photo.Path, photo.PhotoId.ToString());
                    db.Entry(photo).State = EntityState.Modified;
                    db.SaveChanges();                    

                    return photo;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Delete(int id)
        {
            var db = new DbModelContainer();
            var photo = db.Photos.Find(id);
            if (photo != null)
            {
                try
                {
                    db.Entry(photo).State = EntityState.Deleted;
                    db.SaveChanges();
                }
                catch (Exception) { }
            }
        }

        public Photo Find(int id)
        {
            using (var db = new DbModelContainer())
            {
                return db.Photos.FirstOrDefault(p => p.PhotoId == id);
            }
        }

        public IQueryable<Photo> GetPhotos()
        {
            var db = new DbModelContainer();
            return db.Photos;
        }

        public void Save(Photo photo)
        {
            var db = new DbModelContainer();
            db.Entry(photo).State = EntityState.Modified;
            db.SaveChanges();
        }

        public IQueryable<Photo> GetPhotosByAlbumId(int id)
        {
            var db = new DbModelContainer();
            var photos = db.Photos.Where(p => p.AlbumId == id);
            return photos;
        }
    }
}