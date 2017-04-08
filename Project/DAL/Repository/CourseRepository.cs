using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Windows;
using Project.Abstract;

namespace Project.Models
{
    public class CourseRepository : ICourseRepository
    {
        private ApplicationDbContext context;
        public CourseRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        public void Create(HttpPostedFileBase src, Course create, string[] teachers)
        {
            if (src != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(src.FileName);
                //сохраняем файл в папку Files в проекте
                src.SaveAs(HttpContext.Current.Server.MapPath(@"~\Images\" + fileName));
                create.ImagePath = @"~\Images\" + fileName;

            }

            if(create.SrartDate>create.EndDate)
            {
                throw new Exception("Дата початку не повинна бути встановлена раніше кінця кінця курсу");
            }
            //else if()

            if (teachers != null)
            {
                create.Teachers = new List<Teacher>();
                foreach (var teacher in teachers)
                {
                    var teacherToAdd = context.Teachers.Find(int.Parse(teacher));
                    create.Teachers.Add(teacherToAdd);
                }
            }

            context.Courses.Add(create);
        }

        public void Delete(int id)
        {
            var course = context.Courses.Find(id);
            if (course != null)
            {
                context.Courses.Remove(course);
            }
        }
        public Course Detail(int id)
        {
            return context.Courses.Find(id);
        }

        public List<Course> GetAll()
        {
            return context.Courses.ToList<Course>();
        }

        public void Update(HttpPostedFileBase src, Course update, string[] teachers)
        {
            var newCourse = Detail(update.CourseId);
            newCourse.CategoryId = update.CategoryId;
            newCourse.DescriptionCourse = update.DescriptionCourse;
            newCourse.SrartDate = update.SrartDate;
            newCourse.EndDate = update.EndDate;
            newCourse.Title = update.Title;
            if (newCourse.SrartDate > newCourse.EndDate)
            {
                throw new Exception("Дата початку не повинна бути встановлена раніше кінця кінця курсу");
            }

            if (src != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(src.FileName);
                //сохраняем файл в папку Files в проекте
                src.SaveAs(HttpContext.Current.Server.MapPath(@"~\Images\" + fileName));
                newCourse.ImagePath = @"~\Images\" + fileName;

                //context.Courses.Add(create);
            }
            newCourse.Teachers.Clear();
            if (teachers != null)
            {
                update.Teachers = new List<Teacher>();
                foreach (var teacher in teachers)
                {
                    var teacherToAdd = context.Teachers.Find(int.Parse(teacher));
                    newCourse.Teachers.Add(teacherToAdd);
                }
            }
            context.Entry(newCourse).State = EntityState.Modified;
        }

        public SelectList CategoryList(object select = null)
        {
            var list = new SelectList(context.Categories, "CategoryId", "Title", select);
            return list;
        }

    }
}