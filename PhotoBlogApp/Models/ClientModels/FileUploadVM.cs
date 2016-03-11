using System.Collections.Generic;
using System.Web;

namespace PhotoBlogApp.Models.ClientModels
{
    public class FileUploadVm
    {
        public int AlbumId { get; set; }
        public IEnumerable<HttpPostedFileBase> UploadedFiles { get; set; }
        public HttpFileCollectionBase UploadedRequestFiles { get; set; }
        public string Login { get; set; }
    }

    public class FileUploadM
    {
        public int AlbumId { get; set; }
        public HttpPostedFileBase UploadedFile { get; set; }        
        public string Login { get; set; }
    }
}