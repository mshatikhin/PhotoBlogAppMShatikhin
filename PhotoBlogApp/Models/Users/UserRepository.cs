using System;
using System.Linq;
using System.Net;
using PhotoBlogApp.Core;
using PhotoBlogApp.Helpers;

namespace PhotoBlogApp.Models.Users
{
    public class UserRepository : IUserRepository
    {
        public User Save(NetworkCredential credentials)
        {
            var usr = new User
            {
                Login = credentials.UserName,
                Password = AppHelper.HashAndSolt(credentials.Password),
                Email = "",
                IsAdmin = false
            };
            try
            {
                using (var db = new DbModelContainer())
                {
                    db.Users.Add(usr);
                    db.SaveChanges();
                    return usr;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool ExistUser(string userName)
        {
            using (var db = new DbModelContainer())
            {
                var usr = db.Users.FirstOrDefault(user => user.Login.ToLower() == userName);
                return usr != null;
            }
        }

        public User GetUserByLogin(string login)
        {
            using (var db = new DbModelContainer())
            {
                var usr = db.Users.FirstOrDefault(user => user.Login.ToLower() == login.ToLower());
                return usr;
            }
        }

        public bool UserIsAdmin(string login)
        {
            using (var db = new DbModelContainer())
            {
                var usr = db.Users.FirstOrDefault(user => user.Login.ToLower() == login.ToLower() && user.IsAdmin);
                return usr != null;
            }
        }

        public bool UserIsModerator(string login)
        {
            using (var db = new DbModelContainer())
            {
                var usr = db.Users.FirstOrDefault(user => user.Login.ToLower() == login.ToLower() && user.IsModerator);
                return usr != null;
            }
        }

        public User[] GetUsers()
        {
            using (var db = new DbModelContainer())
            {                
                return db.Users.ToArray();
            }
        }
    }
}