using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Builders
{
    public interface IMainVmBuilder
    {
        MainViewM BuildPhotos(GetPhotosParams getPhotosParams);
        AlbumsViewM BuildAlbums();
        AlbumPhotosViewM BuildPhotoAlbums(int id);
        bool SavePhoto(ClientModels.PhotoM photo);
        PortfolioViewModel BuildPortfolio();
    }
}