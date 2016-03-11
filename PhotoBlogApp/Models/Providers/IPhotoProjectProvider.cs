using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Providers
{
    public interface IPhotoProjectProvider
    {
        PhotoProjectM GetPhotoProject(int photoProjectId);
        PhotoProjectM[] GetPhotoProjects();
    }
}