using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Position
    {
        public int PositionId { get; set; }
        [Display(Name = "Посада")]
        public string Title { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}