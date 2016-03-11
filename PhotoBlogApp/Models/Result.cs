using System.Collections.Generic;

namespace PhotoBlogApp.Models
{
    public class Result<T>
    {
        public Result()
        {
            IsSuccess = true;
            Errors = new List<string>();
        }

        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }

        public void AddError(string error)
        {
            this.Errors.Add(error);
            this.IsSuccess = false;
        }
    }
}