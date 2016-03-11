namespace PhotoBlogApp.Models.ClientModels
{
    public class RequestM
    {
        public string Name { get; set; }
        public string Coordinates { get; set; }
        public string Comments { get; set; }
        public bool Tfp { get; set; }
        public int[] PhotoProjects { get; set; }
    }
}