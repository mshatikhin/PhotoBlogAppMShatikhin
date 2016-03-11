using System.Data.Entity;
using System.Linq;

namespace PhotoBlogApp.Core.Models.Blogs
{
    public class BlogRepository : IBlogRepository
    {
        public void Add(Blog blog)
        {
            using (var db = new DbModelContainer())
            {
                db.Blogs.Add(blog);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new DbModelContainer())
            {
                var blog = db.Blogs.Find(id);
                db.Entry(blog).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public Blog Find(int id)
        {
            using (var db = new DbModelContainer())
            {
                return db.Blogs.Include(b => b.User).FirstOrDefault(p => p.BlogId == id);
            }
        }

        public IQueryable<Blog> GetAll(bool withHidden)
        {
            var db = new DbModelContainer();
            var dbQuery = db.Blogs.Include(b => b.User);
            if (withHidden)
            {
                return dbQuery;
            }
            return dbQuery.Where(a => !a.Hide);
        }

        public void Save(Blog blog)
        {
            using (var db = new DbModelContainer())
            {
                db.Entry(blog).State = EntityState.Modified;
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
    }
}