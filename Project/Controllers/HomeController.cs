using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using Project.BAL;
using System.Net.Mail;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        Service service = new Service();
        private UnitOfWork unitOfWork;
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public HomeController()
        {
            unitOfWork = new UnitOfWork();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Courses(string sortCourse, string search)
        {
            ViewBag.CourseNow = "date_now";
            ViewBag.FutureCourse = "future_course";
            ViewBag.PastCourse = "past_course";
            ViewBag.AllCourse = "all_course";

            var details = unitOfWork.UsersRepos.Detail(User.Identity);
            ViewBag.Details = details;

            var list = unitOfWork.Courses.GetAll();

            //if(!String.IsNullOrEmpty(search))
            //{
            //    list = list.Where(s => s.Title.Contains(search)).ToList();
            //}

            
            if (!String.IsNullOrEmpty(search))
            {
                list = list.Where(s => s.Title.ToLower().Contains(search.ToLower())).ToList();
                if(list.Count() == 0)
                {
                    ViewBag.MessageResult = "Нажаль пошук не дав результатів. Спробуйте ще раз!";
                }
            }

            switch (sortCourse)
            {
                case "date_now":
                    list = list.Where(s => s.EndDate > DateTime.Now && DateTime.Now > s.SrartDate).ToList();
                    break;
                case "future_course":
                    list = list.Where(s => s.SrartDate > DateTime.Now).ToList();
                    break;
                case "past_course":
                    list = list.Where(s => s.SrartDate < DateTime.Now && s.EndDate < DateTime.Now).ToList();
                    break;
                case "all_course":
                    list = unitOfWork.Courses.GetAll();
                    break;
            }

            return View(list);
        }

        public ActionResult DetailCourse(int id)
        {
            ViewBag.User = UserManager.Users.ToList();
            var detail = unitOfWork.Courses.Detail(id);
            if (detail == null)
            {
                return HttpNotFound();
            }
            return View(detail);
        }

        public ActionResult AddCourse(int id)
        {
            var course = unitOfWork.Courses.Detail(id);

            if (course == null)
            {
                return HttpNotFound();
            }

            ViewBag.Us = UserManager.Users.ToList();
            return View(course);
        }

        [HttpPost, ActionName("AddCourse")]
        [ValidateAntiForgeryToken]
        public ActionResult AddCoursePost(int id)
        {
            try
            {
                unitOfWork.UsersRepos.AddCourseToUser(id, User.Identity);
                unitOfWork.Save();
            }
            catch
            {
                return View("Error");
            }
            return View("SuccessResult");
        }



        [ChildActionOnly]
        public ActionResult LastCourses()
        {
            var last = unitOfWork.Courses.GetAll().OrderByDescending(s => s.CourseId).Take(6);
            return PartialView(last);
        }

        public ActionResult Task(int task, int? page)
        {
            var list = from s in unitOfWork.TaskRepos.GetAll()
                       where s.CourseId == task
                       select s;
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        [ChildActionOnly]
        public ActionResult TaskList(int task)
        {
            var list = from s in unitOfWork.TaskRepos.GetAll()
                       where s.CourseId == task
                       select s;
            return PartialView(list);
        }
        [ChildActionOnly]
        public ActionResult TeacherList()
        {
            var list = unitOfWork.TeacherRepos.GetAll();
            return PartialView(list);
        }
        public ActionResult DetailTeacher(int id)
        {
            var detail = unitOfWork.TeacherRepos.Detail(id);
            if(detail == null)
            {
                return HttpNotFound();
            }
            return View(detail);
        }

        /*
         * Пример загрузки файла
         * нужно изменить
         */
        //public ActionResult Download()
        //{
        //    string file = "Random.jpg";

        //    string filepath = Server.MapPath(@"~\Images\" + file);
        //    byte[] filedata = System.IO.File.ReadAllBytes(filepath);
        //    string content = MimeMapping.GetMimeMapping(filepath);
        //    var cd = new System.Net.Mime.ContentDisposition
        //    {
        //        FileName = file,
        //        Inline = false
        //    };
        //    Response.AppendHeader("Content-Disposition", cd.ToString());

        //    return File(filedata, content);
        //}

        public ActionResult Download(string filepath)
        {
            string file = filepath;
            filepath = Server.MapPath(@"~\Files\" + file);

            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string content = MimeMapping.GetMimeMapping(filepath);
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = file,
                Inline = false
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, content);
        }
        [ChildActionOnly]
        public ActionResult Send()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Send(string from, string yourName, string body)
        {
            service.SendEmail(from, yourName, body);
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}