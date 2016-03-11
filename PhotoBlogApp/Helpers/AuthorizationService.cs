using System.Linq;
using PhotoBlogApp.Core;

namespace PhotoBlogApp.Helpers
{
    public class AuthorizationService
    {
        public virtual bool ValidateUser(string username, string password)
        {
            using (var db = new DbModelContainer())
            {
                password = AppHelper.HashAndSolt(password);
                var user = db.Users.FirstOrDefault(x => x.Login == username && x.Password == password);
                if (user != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}