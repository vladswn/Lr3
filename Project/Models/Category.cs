using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Display(Name = "Категорія")]
        [Required]
        public string Title { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}