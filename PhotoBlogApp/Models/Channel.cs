using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBlogApp.Models
{
    public class Channel
    {
        //Заголовок сайта - источника.
        public string Title { get; set; }

        //Описание сайта - источника.
        public string Description { get; set; }

        //Ссылка на сайт источника.
        public string Link { get; set; }

        // копирайт.
        public string Copyright { get; set; }

        public Channel()
        {
            Title = "";
            Description = "";
            Link = "";
            Copyright = "";
        }
    }

    public class Items
    {
        //Заголовок сообщения.
        public string Title { get; set; }
        //Ссылка на страницу сообщения в интернете.
        public string Link { get; set; }
        //Краткий обзор сообщения.
        public string Description { get; set; }
        //Дата публикации сообщения.
        public string PubDate { get; set; }

        public Items()
        {
            Title = "";
            Link = "";
            Description = "";
            PubDate = "";
        }
    }

    public class ImageOfChanel
    {
        //Заголовок изображения.
        public string ImgTitle { get; set; }

        //Ссылка на изображение.
        public string ImgLink { get; set; }

        //URL адрес сообщения.
        public string ImgURL { get; set; }

        public ImageOfChanel()
        {
            ImgTitle = "";
            ImgLink = "";
            ImgURL = "";
        }
    }
}
