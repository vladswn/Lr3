using Project.Abstract;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project.Repository
{
    public class TeacherRepository : ITeacherRepository
    {
        private ApplicationDbContext context;
        public TeacherRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        public void Create(HttpPostedFileBase src, Teacher create)
        {
            if (src != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(src.FileName);
                //сохраняем файл в папку Files в проекте
                src.SaveAs(HttpContext.Current.Server.MapPath(@"~\Images\" + fileName));
                create.ImagePath = @"~\Images\" + fileName;

            }

            context.Teachers.Add(create);
        }

        public void Delete(int id)
        {
            var teacher = context.Teachers.Find(id);
            if (teacher != null)
            {
                context.Teachers.Remove(teacher);
            }
        }
        public Teacher Detail(int id)
        {
            return context.Teachers.Find(id);
        }

        public List<Teacher> GetAll()
        {
            return context.Teachers.ToList<Teacher>();
        }

        public void Update(HttpPostedFileBase src, Teacher update)
        {
            if (src != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(src.FileName);
                //сохраняем файл в папку Files в проекте
                src.SaveAs(HttpContext.Current.Server.MapPath(@"~\Images\" + fileName));
                update.ImagePath = @"~\Images\" + fileName;

                //context.Courses.Add(create);
            }
            context.Entry(update).State = EntityState.Modified;
        }
    }
}