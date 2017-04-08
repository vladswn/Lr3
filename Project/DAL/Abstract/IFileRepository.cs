using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Project.Abstract
{
    public interface IFileRepository
    {
        List<File> GetAll();
        File Detail(int id);
        void Create(HttpPostedFileBase src, File create);
        void Delete(int id);
        void Update(HttpPostedFileBase src, File update);
    }
}
