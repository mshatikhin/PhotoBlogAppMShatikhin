using System.Linq;
using PhotoBlogApp.Core;

namespace PhotoBlogApp.Models.Service
{
    public interface IPhotoProjectRepository
    {
        void Add(PhotoProject photoProject);
        void Delete(int id);
        PhotoProject Find(int id);
        IQueryable<PhotoProject> GetPhotoProjects(bool withDisabled);
        void Save(PhotoProject photoProject);
        PhotoProject GetPhotoProject(int id, bool withDisabled);
    }
}