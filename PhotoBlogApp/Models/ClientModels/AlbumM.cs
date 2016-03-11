namespace PhotoBlogApp.Models.ClientModels
{
    public class AlbumM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public PhotoM[] Photos { get; set; }
        public int AlbumId { get; set; }
        public int Year { get; set; }
        public string TitleUrl { get; set; }
    }
}