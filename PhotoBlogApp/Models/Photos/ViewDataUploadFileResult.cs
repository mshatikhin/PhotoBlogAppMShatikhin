namespace PhotoBlogApp.Models.Photos
{
    public class ViewDataUploadFileResult
    {        
        public string SavedFileName { get; set; }
        public string Title { get; set; }
        public string FullPath { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Error { get; set; }
    }
}