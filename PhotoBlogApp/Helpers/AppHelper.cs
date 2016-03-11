using System.Security.Cryptography;
using System.Text;

namespace PhotoBlogApp.Helpers
{
    public static class AppHelper
    {
        public static string HashAndSolt(string password)
        {
            const string solt = "365faces";
            password = password + solt;
            var hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(password));
            var str = "";
            var numArray = hash;
            var index = 0;
            while (index < numArray.Length)
            {
                var num = numArray[index];
                str = str + num.ToString("x2");
                checked
                {
                    ++index;
                }
            }
            return str;
        }
    }
}