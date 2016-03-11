namespace PhotoBlogApp.Models.ClientModels
{
    public class GetPhotosParams
    {
        public int AlbumId { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public int? NumberDay { get; set; }
    }
}