//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PhotoBlogApp.Core
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Blogs = new HashSet<Blog>();
            this.Albums = new HashSet<Album>();
        }
    
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public string Password { get; set; }
        public bool IsModerator { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
    }
}