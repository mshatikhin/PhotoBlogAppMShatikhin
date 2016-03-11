using System.Linq;
using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Providers
{
    public class PhotoProjectProvider : IPhotoProjectProvider
    {
        private readonly IDataCache dataCache;

        public PhotoProjectProvider(IDataCache dataCache)
        {
            this.dataCache = dataCache;
        }

        public PhotoProjectM GetPhotoProject(int photoProjectId)
        {
            var d = dataCache.GetPhotoProjects();
            return d[photoProjectId];
        }

        public PhotoProjectM[] GetPhotoProjects()
        {
            return dataCache.GetPhotoProjects().Values.ToArray();
        }
    }
}