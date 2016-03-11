namespace PhotoBlogApp.Models.ClientModels
{
    public class AlbumPhotosViewM
    {
        public AlbumM Album { get; set; }
        public PhotoM[] Photos { get; set; }
        public AlbumM[] Albums { get; set; }
    }
}