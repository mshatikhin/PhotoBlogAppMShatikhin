using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Providers
{
    public interface IAlbumProvider
    {
        AlbumM GetAlbum(string albumName);
        AlbumM GetAlbum(int albumId);
        AlbumM[] GetAlbums();
    }
}