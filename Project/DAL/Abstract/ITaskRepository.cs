using Project.Models;
using System;
using System.Collections.Generic;



namespace Project.Abstract
{
    public interface ITaskRepository
    {
        List<Task> GetAll();
        Task Detail(int id);
        void Create(Task create, int[] file, string[] selectedVideo);
        void Delete(int id);
        void Update(Task update, int[] file, string[] selectedVideo);
    }
}
