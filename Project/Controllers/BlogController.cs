using PagedList;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class BlogController : Controller
    {
        private UnitOfWork unitOfWork;
        const int pageSize = 5;

        public BlogController()
        {
            unitOfWork = new UnitOfWork();
        }

        public ActionResult Posts(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var list = unitOfWork.BlogRepos.GetAll().OrderByDescending(s=> s.BlogId);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Post(int id)
        {
            var post = unitOfWork.BlogRepos.Detail(id);
            return View(post);
        }

        [ChildActionOnly]
        public ActionResult LastPost()
        {
            var list = unitOfWork.BlogRepos.LastPost();
            return PartialView(list);
        }

        [ChildActionOnly]
        public ActionResult TopPost()
        {
            var list = unitOfWork.BlogRepos.TopNews();
            return PartialView(list);
        }

        [ChildActionOnly]
        public ActionResult LastNews()
        {
            var list = unitOfWork.BlogRepos.LastNews();
            return PartialView(list);
        }
        //[ChildActionOnly]
        public ActionResult Comment(int? blogId, int? page)
        {
            ViewBag.BlogId = blogId;
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            var list = unitOfWork.CommentRepos.GetAll().Where(s => s.BlogId == blogId);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Items", list.ToPagedList(pageNumber, pageSize));
            }
            return PartialView(list.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult Comment(Comment comment, string returnUrl, int blogId)
        {
            DateTime date = DateTime.Now;
            comment = new Comment
            {
                DatePosted = date,
                Body = comment.Body,
                UserName = User.Identity.Name,
                BlogId = blogId
            };
            if (!String.IsNullOrEmpty(comment.Body))
            {
                unitOfWork.CommentRepos.Create(comment);
                unitOfWork.Save();
            }
            return Redirect(returnUrl);
        }

        public List<Comment> GetItemsPage(int? blogId, int page = 1)
        {
            var itemToSkip = page * pageSize;

            return unitOfWork.CommentRepos.GetAll().Where(s=> s.BlogId == blogId).OrderByDescending(s => s.CommentiD).Skip(itemToSkip).Take(pageSize).ToList();
            //return context.Comments.OrderByDescending(s => s.CommentiD).Skip(itemToSkip).Take(pageSize).ToList();
        }

    }
}