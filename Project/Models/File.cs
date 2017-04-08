using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class File
    {
        private string name;
        public int FileId { get; set; }
        [Display(Name = "Розташування")]
        public string FilePath { get; set; }
        [Display(Name = "Ім'я")]
        public string Name
        {
            get
            {
                //string[] words = FilePath.Replace(@"\", " ").Split();
                //name = words.Last();
                name = FilePath.Substring(FilePath.LastIndexOf(@"\") + 1);
                return name;
            }
        }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}