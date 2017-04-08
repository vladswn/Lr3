using Project.Abstract;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project.Repository
{
    public class TaskRepository : /*IRepository<Task>*/ ITaskRepository
    {
        private ApplicationDbContext context;
        public TaskRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public void Create(Task create, int[] files, string[] selectedVideo)
        {
            if (create != null && files != null)
            {
                foreach (var item in context.Files.Where(s => files.Contains(s.FileId)))
                {
                    create.Files.Add(item);
                }
            }
            if (selectedVideo != null)
            {
                create.Videos = new List<Video>();
                foreach (var video in selectedVideo)
                {
                    var videoToAdd = context.Videos.Find(int.Parse(video));
                    create.Videos.Add(videoToAdd);
                }
            }



            context.Tasks.Add(create);
        }

        public void Delete(int id)
        {
            var task = context.Tasks.Find(id);
            if (task != null)
            {
                context.Tasks.Remove(task);
            }
        }

        public Task Detail(int id)
        {
            return context.Tasks.Find(id);
        }

        public List<Task> GetAll()
        {
            return context.Tasks.ToList<Task>();
        }

        public void Update(Task update, int[] files, string[] selectedVideo)
        {
            var newTask = Detail(update.TaskId);
            newTask.Title = update.Title;
            newTask.CourseId = update.CourseId;
            newTask.Content = update.Content;


            newTask.Files.Clear();
            if (files != null)
            {
                foreach (var item in context.Files.Where(s => files.Contains(s.FileId)))
                {
                    newTask.Files.Add(item);
                }
            }
            newTask.Videos.Clear();
            if (selectedVideo != null)
            {
                update.Videos = new List<Video>();
                foreach (var video in selectedVideo)
                {
                    var videoToAdd = context.Videos.Find(int.Parse(video));
                    newTask.Videos.Add(videoToAdd);
                }
            }

            context.Entry(newTask).State = EntityState.Modified;
        }

    }
}