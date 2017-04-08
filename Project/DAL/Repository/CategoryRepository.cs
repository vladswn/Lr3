using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Project.Models
{
    public class CategoryRepository : IRepository<Category>
    {
        private ApplicationDbContext context;
        public CategoryRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        public void Create(Category create)
        {
            context.Categories.Add(create);
        }

        public void Delete(int id)
        {
            var category = context.Categories.Find(id);
            if(category != null)
            {
                context.Categories.Remove(category);
            }
        }

        public Category Detail(int id)
        {
            return context.Categories.Find(id);
        }

        public List<Category> GetAll()
        {
            return context.Categories.ToList<Category>();
        }

        public void Update(Category update)
        {
            context.Entry(update).State = EntityState.Modified;
        }
    }
}