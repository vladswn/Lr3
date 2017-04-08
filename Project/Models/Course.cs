using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        [Required(ErrorMessage ="Обов'язково для заповенння!")]
        [Display(Name ="Назва курсу")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Обов'язково для заповнення!")]
        [Display(Name ="Інформація про курс")]
        public string DescriptionCourse { get; set; }
        [Display(Name ="Зображення")]
        public string ImagePath { get; set; }
        [Required(ErrorMessage ="Дата початку для заповнення обов'язкова!")]
        [Display(Name = "Дата початку курсу")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SrartDate { get; set; }
        [Required(ErrorMessage = "Дата закінчення для заповнення обов'язкова!")]
        [Display(Name = "Дата закінчення курсу")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime Duration { get; set; }
        [Required(ErrorMessage = "Категорія обов'язкова!")]
        [Display(Name = "Категорія")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}