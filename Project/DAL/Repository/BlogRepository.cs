using Project.Abstract;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private ApplicationDbContext context;
        public BlogRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        public void Create(HttpPostedFileBase src, Blog create, System.Security.Principal.IIdentity identity)
        {
            create.CreateTime = DateTime.Now;
            create.UserName = Microsoft.AspNet.Identity.IdentityExtensions.GetUserName(identity);
            if (src != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(src.FileName);
                //сохраняем файл в папку Files в проекте
                src.SaveAs(HttpContext.Current.Server.MapPath(@"~\Images\" + fileName));
                create.ImagePath = @"~\Images\" + fileName;

            }

            context.Blogs.Add(create);
        }

        public void Delete(int id)
        {
            var blog = context.Blogs.Find(id);
            if (blog != null)
            {
                context.Blogs.Remove(blog);
            }
        }
        public Blog Detail(int id)
        {
            return context.Blogs.Find(id);
        }

        public List<Blog> GetAll()
        {
            return context.Blogs.ToList<Blog>();
        }

        public List<Blog> LastPost()
        {
            return context.Blogs.OrderByDescending(s=> s.BlogId).Take(10).ToList<Blog>();
        }
        public List<Blog> LastNews()
        {
            return context.Blogs.OrderByDescending(s => s.BlogId).Take(4).ToList<Blog>();

        }
        public List<Blog> TopNews()
        {
             return context.Blogs.OrderByDescending(s => s.Comments.Count()).Take(4).ToList<Blog>();
        }

        public void Update(HttpPostedFileBase src, Blog update, System.Security.Principal.IIdentity identity)
        {
            var newBlog = Detail(update.BlogId);
            newBlog.CreateTime = update.CreateTime;
            newBlog.Description = update.Description;
            newBlog.EditTime = DateTime.Now;
            newBlog.ShortDescription = update.ShortDescription;
            newBlog.Title = update.Title;
            newBlog.UserEditBlog = Microsoft.AspNet.Identity.IdentityExtensions.GetUserName(identity);
            newBlog.UserName = update.UserName;
            
            if (src != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(src.FileName);
                //сохраняем файл в папку Files в проекте
                src.SaveAs(HttpContext.Current.Server.MapPath(@"~\Images\" + fileName));
                newBlog.ImagePath = @"~\Images\" + fileName;

            }
            context.Entry(newBlog).State = EntityState.Modified;
        }
    }
}