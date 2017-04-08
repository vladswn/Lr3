using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Display(Name = "Прізвище")]
        public string Surname { get; set; }
        [Display(Name = "По батькові")]
        public string Patronymic { get; set; }
        [Display(Name = "Телефон")]
        public int Telephone { get; set; }
        [Display(Name = "E-mail")]
        public string E_mail { get; set; }
        [Display(Name = "Розташування")]
        public string ImagePath { get; set; }
        //public string Position { get; set; }
        [Display(Name = "Посада")]
        public int PositionId { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }
        private string fullName;
        [Display(Name = "ПІБ")]
        public string FullName
        {
            get
            {
                fullName = Surname + " " + Name.Substring(0, 1) + ". " + Patronymic.Substring(0, 1) + ".";
                return fullName;
            }
        }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual Position Position { get; set; }
    }
}