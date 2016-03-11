using PhotoBlogApp.Core;
using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Service
{
    public interface IPhotoProjectService
    {
        PhotoProjectM[] GetPhotoProjects();
        PhotoProjectM BuildPhotoProjectM(PhotoProject photoProject);
    }
}