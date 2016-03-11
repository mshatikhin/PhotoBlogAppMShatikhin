using System;
using System.Linq;
using PhotoBlogApp.Controllers;
using PhotoBlogApp.Models.ClientModels;
using PhotoBlogApp.Models.ClientModels.TimeLine;

namespace PhotoBlogApp.Models.Builders
{
    public class TimeLineBuilder : ITimeLineBuilder
    {
        public object Build(PostM[] posts)
        {
            var timeLineDates = posts
                .Select(p =>
                {
                    var headline = string.Format("<a href='#/blog/post/{0}' class='time-line__link'>{1}</a>", p.PostId, p.Title);
                    var link = string.Format("<a href='#/blog/post/{0}' class='time-line__link'>читать полностью...</a>", p.PostId);
                    var thumbnail = BuildThumbnailUrl(p.ImageUrl);
                    var buildAsset = BuildAsset("", "", p.ImageUrl, thumbnail);
                    return BuildTimeLineDate(buildAsset, p.Date.ToString("yyyy,MM,dd"), p.DateEnd.ToString("yyyy,MM,dd"), headline, string.Format("{0}<br/><br/>{1}", p.Description, link), "", "adventure-time-line");
                })
                .ToArray();

            var model = new
            {
                timeline = BuildTimeLineData(timeLineDates, new TimeLineEra[0], "ћой путь", "default", "<p>¬ этих заметках вы можете найти часть мен€, узнать что то новое.</p>", null)
            };
            return model;
        }

        private static string BuildThumbnailUrl(string imageUrl)
        {
            var hasImg = imageUrl.IndexOf("jpg", StringComparison.InvariantCultureIgnoreCase) != -1;
            return hasImg ? imageUrl : "/Content/img/blogicon.jpg";
        }


        private static TimeLine BuildTimeLineData(TimeLineDate[] timeLineDates, TimeLineEra[] timeLineEras, string headline, string type, string text, Asset asset)
        {
            return new TimeLine
            {
                headline = headline,
                type = type,
                text = text,
                asset = asset,
                date = timeLineDates,
                era = timeLineEras
            };
        }

        private static TimeLineDate BuildTimeLineDate(Asset asset, string startDate, string endDate, string headline, string text, string tag, string classname)
        {
            return new TimeLineDate
            {
                startDate = startDate,
                endDate = endDate,
                headline = headline,
                text = text,
                tag = tag,
                classname = classname,
                asset = asset
            };
        }

        private static Asset BuildAsset(string caption, string credit, string media, string thumbnail)
        {
            return new Asset
            {
                caption = caption,
                credit = credit,
                media = media,
                thumbnail = thumbnail
            };
        }
    }
}