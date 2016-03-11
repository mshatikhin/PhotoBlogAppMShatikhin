namespace PhotoBlogApp.Models.ClientModels
{
    public class PhotoM
    {
        public int PhotoId { get; set; }
        public string ThumbUrl { get; set; }
        public string SmallUrl { get; set; }
        public string Url { get; set; }
        public string FullUrl { get; set; }
        public string Title { get; set; }
        public string AlbumName { get; set; }
        public string AlbumDescription { get; set; }
        public bool IsWide { get; set; }        
    }
}