using System.Linq;

namespace PhotoBlogApp.Core.Models.Requests
{
    public interface IRequestRepository
    {
        void Add(Request request);
        void Delete(int id);
        Request Find(int id);
        IQueryable<Request> GetRequests();
        void Save(Request request);
    }
}
