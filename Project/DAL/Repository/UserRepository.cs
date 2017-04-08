using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace Project.Models
{
    public class UserRepository
    {
        private ApplicationDbContext context;
        public UserRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
        
        public ApplicationUser Detail(System.Security.Principal.IIdentity ids)
        {
            var idUser = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(ids);
            var us = context.Users.Find(idUser);
            return us;
        }

        public void AddCourseToUser(int? id, System.Security.Principal.IIdentity ids)
        {
            var detail = Detail(ids);
            var course = context.Courses.Find(id);

            detail.Courses.Add(course);
        }
        public void DeleteCourseFromUser(int? id, System.Security.Principal.IIdentity ids)
        {
            var detail = Detail(ids);
            var course = context.Courses.Find(id);
            if (detail != null)
            {
                detail.Courses.Remove(course);
            }
        }

    }
}