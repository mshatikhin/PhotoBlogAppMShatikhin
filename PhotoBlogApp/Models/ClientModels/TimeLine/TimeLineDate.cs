namespace PhotoBlogApp.Models.ClientModels.TimeLine
{
    public class TimeLineDate
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string headline { get; set; }
        public string text { get; set; }
        public string tag { get; set; }
        public string classname { get; set; }
        public Asset asset { get; set; }
    }
}