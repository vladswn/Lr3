using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Project.Controllers
{
    public class HomeAPIController : ApiController
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET api/HomeAPI
        public IEnumerable<Category> GetCategories()
        {
            return context.Categories;
        }
      

        //public IEnumerable<Teacher> GetTeacher()
        //{
        //    return context.Teachers;
        //}
    }
}
