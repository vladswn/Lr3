using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Comment
    {
        public int CommentiD { get; set; }
        [Display(Name = "Дата розміщення")]
        public DateTime DatePosted { get; set; }
        [Display(Name = "Користувач")]
        public string UserName { get; set; }
        [Display(Name = "Коментарій")]
        public string Body { get; set; }
        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; }
    }
}