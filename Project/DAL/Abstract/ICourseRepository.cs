using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Project.Abstract
{
    interface ICourseRepository
    {
        List<Course> GetAll();
        Course Detail(int id);
        void Create(HttpPostedFileBase src, Course create, string[] teachers);
        void Delete(int id);
        void Update(HttpPostedFileBase src, Course update, string[] teachers);
    }
}
