using System;
using PhotoBlogApp.Core;
using PhotoBlogApp.Core.Models.Requests;
using PhotoBlogApp.Models.ClientModels;

namespace PhotoBlogApp.Models.Service
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository requestRepository;

        public RequestService(IRequestRepository requestRepository)
        {
            this.requestRepository = requestRepository;
        }

        public Result<string> Validate(RequestM request)
        {
            var result = new Result<string>();
            if (string.IsNullOrEmpty(request.Name))
            {
                result.AddError("Name");
            }
            if (string.IsNullOrEmpty(request.Coordinates))
            {
                result.AddError("Coordinates");
            }
            return result;
        }

        public void Save(RequestM requestM)
        {
            var request = new Request
            {
                FirstName = requestM.Name,
                ContactInfo = requestM.Coordinates,
                Comment = requestM.Comments,
                Date = DateTime.Now,
                LastName = ""
            };
            requestRepository.Add(request);
        }
    }
}