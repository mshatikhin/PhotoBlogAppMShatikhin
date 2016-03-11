using System.Data.Entity;
using System.Linq;
using PhotoBlogApp.Core;

namespace PhotoBlogApp.Models.Service
{
    public class PhotoProjectRepository : IPhotoProjectRepository
    {
        public void Add(PhotoProject photoProject)
        {
            using (var db = new DbModelContainer())
            {
                db.PhotoProjects.Add(photoProject);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var db = new DbModelContainer())
            {
                var photoProject = db.PhotoProjects.Find(id);
                db.Entry(photoProject).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public PhotoProject Find(int id)
        {
            using (var db = new DbModelContainer())
            {
                return db.PhotoProjects.Include(x => x.Request).FirstOrDefault(p => p.PhotoProjectId == id);
            }
        }

        public IQueryable<PhotoProject> GetPhotoProjects(bool withDisabled)
        {
            var db = new DbModelContainer();
            if (withDisabled)
            {
                return db.PhotoProjects.Include(x => x.Request);
            }
            return db.PhotoProjects.Include(x => x.Request).Where(a => a.Enable);
        }

        public void Save(PhotoProject photoProject)
        {
            using (var db = new DbModelContainer())
            {
                db.Entry(photoProject).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public PhotoProject GetPhotoProject(int id, bool withDisabled)
        {
            using (var db = new DbModelContainer())
            {
                if (withDisabled)
                {
                    return db.PhotoProjects.Include(x => x.Request).SingleOrDefault(a => a.PhotoProjectId == id);
                }
                return db.PhotoProjects.Include(x => x.Request).SingleOrDefault(a => a.PhotoProjectId == id && a.Enable);
            }
        }
    }
}