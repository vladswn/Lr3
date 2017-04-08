using Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project.Repository
{
    public class VideoRepository : IRepository<Video>
    {
        private ApplicationDbContext context;
        public VideoRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        public void Create(Video create)
        {
            context.Videos.Add(create);
        }

        public void Delete(int id)
        {
            var video = context.Videos.Find(id);
            if (video != null)
            {
                context.Videos.Remove(video);
            }
        }

        public Video Detail(int id)
        {
            return context.Videos.Find(id);
        }

        public List<Video> GetAll()
        {
            return context.Videos.ToList<Video>();
        }

        public void Update(Video update)
        {
            context.Entry(update).State = EntityState.Modified;
        }
    }
}