using PhotoBlogApp.Core;
using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Service
{
    public interface IAlbumService
    {
        AlbumM BuildAlbumM(Album album);
        AlbumM[] GetAlbums();        
    }
}