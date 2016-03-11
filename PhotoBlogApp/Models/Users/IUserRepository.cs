using System.Net;
using PhotoBlogApp.Core;

namespace PhotoBlogApp.Models.Users
{
    public interface IUserRepository
    {
        User Save(NetworkCredential credentials);
        bool ExistUser(string userName);
        User GetUserByLogin(string login);
        bool UserIsAdmin(string login);
        bool UserIsModerator(string login);
        User[] GetUsers();
    }
}