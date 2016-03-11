using System.Collections.Generic;

namespace PhotoBlogApp.Models.ClientModels
{
    public class MainViewM
    {
        public PhotoM[] PhotoM { get; set; }
        public int Skip { get; set; }
        public bool Scrolled { get; set; }
        public AlbumM PicturesOfDay { get; set; }
    }
}