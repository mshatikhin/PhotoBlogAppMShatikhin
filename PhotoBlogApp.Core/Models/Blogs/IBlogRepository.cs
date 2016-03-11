using System.Linq;

namespace PhotoBlogApp.Core.Models.Blogs
{
    public interface IBlogRepository
    {
        void Add(Blog blog);
        void Delete(int id);
        Blog Find(int id);
        IQueryable<Blog> GetAll(bool withHidden);
        void Save(Blog blog);        
    }
}
