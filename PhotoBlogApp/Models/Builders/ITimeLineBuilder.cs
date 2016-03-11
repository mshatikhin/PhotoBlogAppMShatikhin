using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Builders
{
    public interface ITimeLineBuilder
    {
        object Build(PostM[] posts);
    }
}
