using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Task
    {
        public int TaskId { get; set; }
        [Display(Name = "Назва")]
        [Required]
        public string Title { get; set; }
        [Display(Name = "Контент")]
        public string Content { get; set; }
        [Display(Name = "Курс")]
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Video> Videos { get; set; }

    }
}