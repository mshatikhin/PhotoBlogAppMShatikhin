namespace PhotoBlogApp.Models.ClientModels
{
    public class PagingM
    {
        public PagingM()
        {
            HasNextPage = true;
            HasPrevPage = true;
        }

        public string NextPage { get; set; }
        public string PrevPage { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPrevPage { get; set; }
    }
}