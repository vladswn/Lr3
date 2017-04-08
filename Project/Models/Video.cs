using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Video
    {
        public int VideoId { get; set; }
        [Display(Name = "Розташування")]
        public string VideoPath { get; set; }
        [Display(Name = "Назва")]
        public string Title { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}