using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Providers
{
    public interface IBlogProvider
    {
        PostM[] GetPosts();
        PostM GetPost(string title);
    }
}