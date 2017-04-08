using Project.Abstract;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project.Repository
{
    public class FileRepository : IFileRepository
    {
        private ApplicationDbContext context;
        public FileRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public void Create(HttpPostedFileBase src, File create)
        {
            if (src != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(src.FileName);
                //сохраняем файл в папку Files в проекте
                src.SaveAs(HttpContext.Current.Server.MapPath(@"~\Files\" + fileName));
                create.FilePath = @"~\Files\" + fileName;

            }

            context.Files.Add(create);
        }

        public void Delete(int id)
        {
            var file = context.Files.Find(id);
            if (file != null)
            {
                var path = HttpContext.Current.Server.MapPath(@"~\Files\" + file.Name);
                System.IO.File.Delete(path);
                context.Files.Remove(file);
            }
        }
        public File Detail(int id)
        {
            return context.Files.Find(id);
        }

        public List<File> GetAll()
        {
            return context.Files.ToList<File>();
        }

        public void Update(HttpPostedFileBase src, File update)
        {
            if (src != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(src.FileName);
                //сохраняем файл в папку Files в проекте
                src.SaveAs(HttpContext.Current.Server.MapPath(@"~\Files\" + fileName));
                update.FilePath = @"~\Files\" + fileName;
            }
            context.Entry(update).State = EntityState.Modified;
        }
    }
}