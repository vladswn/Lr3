using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Models
{
    public class Blog
    {
        public int BlogId { get; set; }
        [Required]
        [Display(Name ="Назва")]
        public string Title { get; set; }
        [Display(Name ="Дата створення")]
        public DateTime CreateTime { get; set; }
        [Display(Name = "Дата редагування")]
        public DateTime? EditTime { get; set; }
        [Display(Name = "Користувач, який вносив останні зміни")]
        public string UserEditBlog { get; set; }
        [Display(Name = "Розташування")]
        public string ImagePath { get; set; }
        [Display(Name = "Ім'я користувача")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Короткий опис")]
        public string ShortDescription { get; set; }
        [AllowHtml]
        [Display(Name = "Зміст")]
        [Required]
        public string Description { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}