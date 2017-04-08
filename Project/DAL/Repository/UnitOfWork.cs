using Project.DAL.Repository;
using Project.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private CategoryRepository category;
        private CourseRepository course;
        private UserRepository user;
        private TaskRepository task;
        private FileRepository file;
        private VideoRepository video;
        private TeacherRepository teacher;
        private PositionRepository position;
        private BlogRepository blog;
        private CommentRepository comment;
        public CategoryRepository Categories
        {
            get
            {
                if (category == null)
                    category = new CategoryRepository(context);

                return category;
            }
        }
        public CourseRepository Courses
        {
            get
            {
                if (course == null)
                    course = new CourseRepository(context);
                return course;
            }
        }
        public UserRepository UsersRepos
        {
            get
            {
                if (user == null)
                    user = new UserRepository(context);
                return user;
            }
        }
        public TaskRepository TaskRepos
        {
            get
            {
                if (task == null)
                    task = new TaskRepository(context);
                return task;
            }
        }
        public FileRepository FileRepos
        {
            get
            {
                if (file == null)
                    file = new FileRepository(context);
                return file;
            }
        }
        public VideoRepository VideoRepos
        {
            get
            {
                if (video == null)
                    video = new VideoRepository(context);

                return video;
            }
        }
        public TeacherRepository TeacherRepos
        {
            get
            {
                if (teacher == null)
                    teacher = new TeacherRepository(context);
                return teacher;
            }
        }
        public PositionRepository PositionRepos
        {
            get
            {
                if (position == null)
                    position = new PositionRepository(context);
                return position;
            }
        }

        public BlogRepository BlogRepos
        {
            get
            {
                if (blog == null)
                    blog = new BlogRepository(context);
                return blog;
            }
        }

        public CommentRepository CommentRepos
        {
            get
            {
                if (comment == null)
                    comment = new CommentRepository(context);
                return comment;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }


        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}