using System.Collections.Generic;
using System.Linq;
using PhotoBlogApp.Models.ClientModels;
using PhotoBlogApp.Models.Service;

namespace PhotoBlogApp.Models
{
    public class DataCache : IDataCache
    {
        private object syncRoot = new object();
        private IDictionary<int, AlbumM> photos;
        private IDictionary<string, PostM> posts;
        private IDictionary<int, PhotoProjectM> photoProjects;
        private readonly IAlbumService albumService;
        private readonly IBlogService blogService;
        private readonly IPhotoProjectService photoProjectService;

        public DataCache(
            IAlbumService albumService,
            IBlogService blogService, 
            IPhotoProjectService photoProjectService)
        {
            this.albumService = albumService;
            this.blogService = blogService;
            this.photoProjectService = photoProjectService;
        }

        public void ReloadData()
        {
            lock (syncRoot)
            {
                photos = BuildDictionaryAlbums();
                posts = BuildDictionaryPosts();
                photoProjects = BuildDictionaryPhotoProjects();
            }
        }

        private IDictionary<string, PostM> BuildDictionaryPosts()
        {
            var postMs = blogService.GetPosts();
            return postMs.ToDictionary(p => p.TitleUrl);
        }

        private IDictionary<int, AlbumM> BuildDictionaryAlbums()
        {
            var albums = albumService.GetAlbums();
            return albums.ToDictionary(p => p.AlbumId);
        }

        private IDictionary<int, PhotoProjectM> BuildDictionaryPhotoProjects()
        {
            var photoProjectMs = photoProjectService.GetPhotoProjects();
            return photoProjectMs.ToDictionary(p => p.PhotoProjectId);
        }

        public IDictionary<int, AlbumM> GetAlbums()
        {
            lock (syncRoot)
            {
                return photos ?? (photos = BuildDictionaryAlbums());
            }
        }

        public IDictionary<string, PostM> GetPosts()
        {
            lock (syncRoot)
            {
                return posts ?? (posts = BuildDictionaryPosts());
            }
        }

        public IDictionary<int, PhotoProjectM> GetPhotoProjects()
        {
            lock (syncRoot)
            {
                return photoProjects ?? (photoProjects = BuildDictionaryPhotoProjects());
            }
        }
    }
}