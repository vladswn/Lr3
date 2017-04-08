using Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project.Repository
{
    public class PositionRepository : IRepository<Position>
    {
        private ApplicationDbContext context;
        public PositionRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        public void Create(Position create)
        {
            context.Positions.Add(create);
        }

        public void Delete(int id)
        {
            var position = context.Positions.Find(id);
            if (position != null)
            {
                context.Positions.Remove(position);
            }
        }

        public Position Detail(int id)
        {
            return context.Positions.Find(id);
        }

        public List<Position> GetAll()
        {
            return context.Positions.ToList<Position>();
        }

        public void Update(Position update)
        {
            context.Entry(update).State = EntityState.Modified;
        }
    }
}