using System.Linq;
using PhotoBlogApp.Core;
using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Service
{
    public class PhotoProjectService : IPhotoProjectService
    {
        private readonly IPhotoProjectRepository photoProjectRepository;

        public PhotoProjectService(IPhotoProjectRepository photoProjectRepository)
        {
            this.photoProjectRepository = photoProjectRepository;
        }

        public PhotoProjectM[] GetPhotoProjects()
        {
            var photoProjects = photoProjectRepository
                   .GetPhotoProjects(false)
                   .OrderByDescending(a => a.Order)
                   .ToList()
                   .Select(BuildPhotoProjectM)
                   .ToArray();
            return photoProjects;
        }

        public PhotoProjectM BuildPhotoProjectM(PhotoProject arg)
        {
            return new PhotoProjectM
            {
                PhotoProjectId = arg.PhotoProjectId,
                Name = arg.Name,
                Description = arg.Description
            };
        }
    }
}