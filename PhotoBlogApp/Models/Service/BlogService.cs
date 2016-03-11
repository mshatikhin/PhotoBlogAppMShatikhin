using System.Linq;
using PhotoBlogApp.Core;
using PhotoBlogApp.Core.Models.Blogs;
using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Service
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            this.blogRepository = blogRepository;
        }

        public PostM BuildPostM(Blog post)
        {
            return new PostM
            {
                PostId = post.BlogId,
                HTML = post.ContentText,
                Title = post.HeaderName,
                Author = post.User.Name ?? "anonim",
                Description = post.Description,
                ImageUrl = post.ImageUrl,
                Date = post.DatePublish,
                DateEnd = post.DateEnd ?? post.DatePublish,
                TitleUrl = post.TranslitName
            };
        }

        public PostM[] GetPosts()
        {
            var posts = blogRepository.GetAll(withHidden: false)
                .OrderByDescending(p => p.DatePublish)
                .ToList()
                .Select(BuildPostM)
                .ToArray();
            return posts;
        }
    }
}