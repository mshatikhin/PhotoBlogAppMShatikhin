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
    
    public partial class Blog
    {
        public int BlogId { get; set; }
        public string HeaderName { get; set; }
        public string ContentText { get; set; }
        public int UserId { get; set; }
        public System.DateTime DatePublish { get; set; }
        public bool Hide { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public string TranslitName { get; set; }
    
        public virtual User User { get; set; }
    }
}
