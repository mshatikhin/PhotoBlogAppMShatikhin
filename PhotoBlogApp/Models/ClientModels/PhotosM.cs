namespace PhotoBlogApp.Models.ClientModels
{
    public class PhotosM
    {
        public PhotoM[] Photos { get; set; }
        public int Skip { get; set; }
        public bool Scrolled { get; set; }
        public string Temp { get; set; }
    }
}