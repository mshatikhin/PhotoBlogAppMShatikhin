using PhotoBlogApp.Controllers;

namespace PhotoBlogApp.Models.ClientModels.TimeLine
{
    public class TimeLine
    {
        public string headline { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public Asset asset { get; set; }
        public TimeLineDate[] date { get; set; }
        public TimeLineEra[] era { get; set; }

    }
}