using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Service
{
    public interface IRequestService
    {
        Result<string> Validate(RequestM request);
        void Save(RequestM request);
    }
}