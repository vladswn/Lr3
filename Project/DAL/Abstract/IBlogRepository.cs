using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Project.Abstract
{
    interface IBlogRepository
    {
        List<Blog> GetAll();
        Blog Detail(int id);
        void Create(HttpPostedFileBase src, Blog create, System.Security.Principal.IIdentity identity);
        void Delete(int id);
        void Update(HttpPostedFileBase src, Blog update, System.Security.Principal.IIdentity identity);
    }
}
