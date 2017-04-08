using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Project.Abstract
{
    interface ITeacherRepository
    {
        List<Teacher> GetAll();
        Teacher Detail(int id);
        void Create(HttpPostedFileBase src, Teacher create);
        void Delete(int id);
        void Update(HttpPostedFileBase src, Teacher update);
    }
}
