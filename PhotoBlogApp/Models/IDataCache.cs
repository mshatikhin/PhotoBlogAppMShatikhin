using System.Collections.Generic;
using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models
{
    public interface IDataCache
    {
        void ReloadData();
        IDictionary<int, AlbumM> GetAlbums();
        IDictionary<string, PostM> GetPosts();
        IDictionary<int, PhotoProjectM> GetPhotoProjects();
    }
}
