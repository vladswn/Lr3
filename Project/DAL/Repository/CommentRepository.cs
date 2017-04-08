using Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project.DAL.Repository
{
    public class CommentRepository : IRepository<Comment>
    {
        private ApplicationDbContext context;
        public CommentRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        public void Create(Comment create)
        {
            context.Comments.Add(create);
        }

        public void Delete(int id)
        {
            var comment = context.Comments.Find(id);
            if (comment != null)
            {
                context.Comments.Remove(comment);
            }
        }

        public Comment Detail(int id)
        {
            return context.Comments.Find(id);
        }

        public List<Comment> GetAll()
        {
            return context.Comments.ToList<Comment>();
        }

        public void Update(Comment update)
        {
            context.Entry(update).State = EntityState.Modified;
        }

        //public List<Comment> GetItemsPage(int page = 1, int pageSize = 5)
        //{
        //    var itemToSkip = page * pageSize;

        //    return context.Comments.OrderByDescending(s => s.CommentiD).Skip(itemToSkip).Take(pageSize).ToList();
        //}
    }
}