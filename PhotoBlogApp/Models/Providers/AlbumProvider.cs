using System.Linq;
using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Providers
{
    public class AlbumProvider : IAlbumProvider
    {
        private readonly IDataCache dataCache;

        public AlbumProvider(IDataCache dataCache)
        {
            this.dataCache = dataCache;
        }

        public AlbumM GetAlbum(string albumName)
        {
            var d = dataCache.GetAlbums();
            return d.Values.FirstOrDefault(a=>a.Name == albumName);
        }

        public AlbumM GetAlbum(int albumId)
        {
            var d = dataCache.GetAlbums();
            return d[albumId];
        }

        public AlbumM[] GetAlbums()
        {
            return dataCache.GetAlbums().Values.ToArray();
        }
    }
}