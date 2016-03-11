using System.Linq;

namespace PhotoBlogApp.Core.Models.Mentions
{
    public interface IMentionRepository
    {
        void Add(Mention mentionEntity);
        IQueryable<Request> GetMentions();
    }
}