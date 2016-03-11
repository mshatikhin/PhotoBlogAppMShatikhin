using PhotoBlogApp.Core;
using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Service
{
    public interface IBlogService
    {
        PostM BuildPostM(Blog post);
        PostM[] GetPosts();        
    }
}