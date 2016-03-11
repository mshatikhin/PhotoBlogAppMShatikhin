using System.Linq;

namespace PhotoBlogApp.Core.Models.Mentions
{
    public class MentionRepository : IMentionRepository
    {
        public void Add(Mention mentionEntity)
        {
            using (var db = new DbModelContainer())
            {
                db.Mentions.Add(mentionEntity);
                db.SaveChanges();
            }
        }

        public IQueryable<Request> GetMentions()
        {
            var db = new DbModelContainer();
            return db.Requests;
        }
    }
}