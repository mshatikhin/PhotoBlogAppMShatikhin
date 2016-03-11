using System.Linq;
using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Providers
{
    public class BlogProvider : IBlogProvider
    {
        private readonly IDataCache dataCache;

        public BlogProvider(IDataCache dataCache)
        {
            this.dataCache = dataCache;
        }

        public PostM[] GetPosts()
        {
            return dataCache.GetPosts().Values.ToArray();
        }

        public PostM GetPost(string title)
        {
            var post = dataCache.GetPosts();
            return post[title];
        }
    }
}