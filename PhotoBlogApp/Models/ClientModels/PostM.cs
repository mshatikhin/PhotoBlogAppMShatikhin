using System;
using System.Web.Mvc;
using PhotoBlogApp.Core;

namespace PhotoBlogApp.Models.ClientModels
{
    public class PostM
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string HTML { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateEnd { get; set; }
        public PagingM PagingM { get; set; }
        public string TitleUrl { get; set; }
    }
}