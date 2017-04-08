using Microsoft.AspNet.Identity.Owin;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using Project.ViewModels;

namespace Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private UnitOfWork unitOfWork;
        public AdminController()
        {
            unitOfWork = new UnitOfWork();
        }

        public AdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }


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

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        // GET: Admin
        public ActionResult Index()
        {
            //var id = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(User.Identity);

            
          //  ViewBag.ID = id.ToString();
            return View();
        }

        public async Task<ActionResult> RoleList()
        {
            var list = await RoleManager.Roles.ToListAsync();
            return View(list);
        }

        public ActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateRole(RoleViewModel roleModel)
        {
            if(ModelState.IsValid)
            {
                var role = new ApplicationRole();
                role.Name = roleModel.Name;
                role.Description = roleModel.Description;

                var result = await RoleManager.CreateAsync(role);
                if(!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                }

                return RedirectToAction("RoleList");
            }
            return View();
        }

        public async Task<ActionResult> EditRole(string id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            var role = await RoleManager.FindByIdAsync(id);

            RoleViewModel viewModel = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRole(RoleViewModel roleModel)
        {
            if (ModelState.IsValid)
            {
                var role = await RoleManager.FindByIdAsync(roleModel.Id);
                role.Name = roleModel.Name;
                role.Description = roleModel.Description;

                await RoleManager.UpdateAsync(role);

                return RedirectToAction("RoleList");
            }
            return View();
        }

        public async Task<ActionResult> DeleteRole(string id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }

            var role = await RoleManager.FindByIdAsync(id);
            return View(role);
        }
        [HttpPost, ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRoleConfirmed(string id)
        {
            if(ModelState.IsValid)
            {
                if(id == null)
                {
                    return HttpNotFound();
                }
                var role = await RoleManager.FindByIdAsync(id);

                var delete = await RoleManager.DeleteAsync(role);

                if(!delete.Succeeded)
                {
                    ModelState.AddModelError("", delete.Errors.First());
                    return View();
                }
                return RedirectToAction("RoleList");
            }
            return View();
        }

        public async Task<ActionResult> UsersList()
        {
            var list = await UserManager.Users.ToListAsync();

            return View(list);
        }

        public async Task<ActionResult> UserDetails(string id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            var user = await UserManager.FindByIdAsync(id);
            ViewBag.RoleList = await UserManager.GetRolesAsync(user.Id);
            return View(user);
        }

        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userViewModel.Email,
                    Email =
                    userViewModel.Email,
                    // Add the Address Info:
                    Name = userViewModel.Name,
                    Surname = userViewModel.Surname
                };

                

                // Then create:
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }


        // GET: /Users/Edit/1
        public async Task<ActionResult> EditUser(string id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);

            EditUserViewModel nd = new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                UserName = user.UserName,
                Surname = user.Surname,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            };

            return View(nd);
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = editUser.Email;
                user.Email = editUser.Email;
                user.Name = editUser.Name;
                user.Surname = editUser.Surname;

                var userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        public async Task<ActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var user = await UserManager.FindByIdAsync(id);
            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteUserConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return HttpNotFound();
                }
                var user = await UserManager.FindByIdAsync(id);

                var delete = await UserManager.DeleteAsync(user);

                if (!delete.Succeeded)
                {
                    ModelState.AddModelError("", delete.Errors.First());
                    return View();
                }
                return RedirectToAction("UsersList");
            }
            return View();
        }

        public ActionResult CategoryList()
        {
            var list = unitOfWork.Categories.GetAll();
            return View(list);
        }

        public ActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Categories.Create(category);
                unitOfWork.Save();
                return RedirectToAction("CategoryList");
            }
            return View(category);
        }

        public ActionResult EditCategory(int id)
        {
            var edit = unitOfWork.Categories.Detail(id);
            if(edit == null)
            {
                return HttpNotFound();
            }
            return View(edit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Categories.Update(category);
                unitOfWork.Save();
                return RedirectToAction("CategoryList");
            }
            return View(category);
        }
        public ActionResult CategoryDetails(int id)
        {
            var detail = unitOfWork.Categories.Detail(id);
            if (detail == null)
            {
                return HttpNotFound();
            }
            return View(detail);
        }

        public ActionResult DeleteCategory(int id)
        {
            var delete = unitOfWork.Categories.Detail(id);
            if(delete == null)
            {
                return HttpNotFound();
            }
            return View(delete);
        }
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategoryConfirmed(int id)
        {
            unitOfWork.Categories.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("CategoryList");
        }

        public ActionResult CorseList()
        {
            var list = unitOfWork.Courses.GetAll();
            return View(list);
        }

        public ActionResult CreateCourse()
        {
            ViewBag.Teachers = unitOfWork.TeacherRepos.GetAll();
            /*ViewBag.CategoryId = */CategoryDropDownList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCourse(HttpPostedFileBase src, Course course, string[] teachers)
        {
            if(ModelState.IsValid)
            {
                ViewBag.Teachers = unitOfWork.TeacherRepos.GetAll();
                CategoryDropDownList();
                CategoryDropDownList();
                try
                {
                    unitOfWork.Courses.Create(src, course, teachers);
                    unitOfWork.Save();
                    return RedirectToAction("CorseList");
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Admin", "CreateCourse"));
                }
            }
            

            //ViewBag.Teachers = unitOfWork.TeacherRepos.GetAll();
            /*ViewBag.CategoryId =*/
            //CategoryDropDownList();

            // ViewBag.CategoryId = unitOfWork.Courses.CategoryList(course.CategoryId);
            //if(ModelState.IsValid)
            //{

                //unitOfWork.Courses.Create(src, course, teachers);
                //unitOfWork.Save();
                //return RedirectToAction("CorseList");

            //}
            return View();
        }

        public ActionResult EditCourse(int id)
        {
            var edit = unitOfWork.Courses.Detail(id);
            if (edit == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CategoryId = unitOfWork.Courses.CategoryList(edit.CategoryId);
            CategoryDropDownList(edit.CategoryId);
            ViewBag.Teachers = unitOfWork.TeacherRepos.GetAll();
            return View(edit);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditCourse(HttpPostedFileBase src, Course edit, string[] teachers)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ViewBag.Teachers = unitOfWork.TeacherRepos.GetAll();
                    CategoryDropDownList(edit.CategoryId);
                    unitOfWork.Courses.Update(src, edit, teachers);
                    unitOfWork.Save();
                    return RedirectToAction("CorseList");
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Admin", "EditCourse"));
                }
            }
            return View();
        }

        public ActionResult CourseDetails(int id)
        {
            var detail = unitOfWork.Courses.Detail(id);
            if(detail == null)
            {
                return HttpNotFound();
            }
            return View(detail);
        }

        public ActionResult DeleteCourse(int id)
        {
            var delete = unitOfWork.Courses.Detail(id);
            if (delete == null)
            {
                return HttpNotFound();
            }
            return View(delete);
        }
        [HttpPost, ActionName("DeleteCourse")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCourseConfirmed(int id)
        {
            unitOfWork.Courses.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("CorseList");
        }

        public ActionResult TaskList()
        {
            var list = unitOfWork.TaskRepos.GetAll();
            return View(list);
        }

        public ActionResult AddTask()
        {
            ViewBag.Files = unitOfWork.FileRepos.GetAll();
            ViewBag.Videos = unitOfWork.VideoRepos.GetAll();
            ViewBag.CourseId = CourseDropDownList();
            return View();
        }

        [HttpPost]
        public ActionResult AddTask(Project.Models.Task task, int[] files, string[] selectedVideo)
        {

            //ViewBag.Files = unitOfWork.FileRepos.GetAll();
            //ViewBag.CourseId = CourseDropDownList();
            unitOfWork.TaskRepos.Create(task, files, selectedVideo);
            unitOfWork.Save();
            return RedirectToAction("TaskList");
        }

        public ActionResult EditTask(int id)
        {
            ViewBag.Files = unitOfWork.FileRepos.GetAll();
            ViewBag.Videos = unitOfWork.VideoRepos.GetAll();
            
            var edit = unitOfWork.TaskRepos.Detail(id);
            if(edit == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = CourseDropDownList(edit.CourseId);
            return View(edit);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditTask(Project.Models.Task task, int[] files, string[] selectedVideo)
        {
            //if(ModelState.IsValid)
            //{
                unitOfWork.TaskRepos.Update(task, files, selectedVideo);
                unitOfWork.Save();
                return RedirectToAction("TaskList");
            //}
            
            //return View(task);
        }

        public ActionResult TaskDetails(int id)
        {
            var detail = unitOfWork.TaskRepos.Detail(id);
            if (detail == null)
            {
                return HttpNotFound();
            }
            return View(detail);
        }
        public ActionResult DeleteTask(int id)
        {
            var delete = unitOfWork.TaskRepos.Detail(id);
            if (delete == null)
            {
                return HttpNotFound();
            }
            return View(delete);
        }
        [HttpPost, ActionName("DeleteTask")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTaskConfirmed(int id)
        {
            unitOfWork.TaskRepos.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("TaskList");
        }

        public ActionResult FileList()
        {
            var list = unitOfWork.FileRepos.GetAll();
            return View(list);
        }

        public ActionResult AddFile()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFile(HttpPostedFileBase src, File file)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.FileRepos.Create(src, file);
                unitOfWork.Save();
                return RedirectToAction("FileList");
            }
            return View();
        }
        public ActionResult EditFile(int id)
        {
            var edit = unitOfWork.FileRepos.Detail(id);
            if(edit == null)
            {
                return HttpNotFound();
            }
            return View(edit);
        }
        [HttpPost]
        public ActionResult EditFile(HttpPostedFileBase src, File file)
        {
            if(ModelState.IsValid)
            {
                unitOfWork.FileRepos.Update(src, file);
                unitOfWork.Save();
                return RedirectToAction("FileList");
            }
            return View(file);
                
        }

        public ActionResult FileDetails(int id)
        {
            var detail = unitOfWork.FileRepos.Detail(id);
            if (detail == null)
            {
                return HttpNotFound();
            }
            return View(detail);
        }

        public ActionResult DeleteFile(int id)
        {
            var delete = unitOfWork.FileRepos.Detail(id);
            if (delete == null)
            {
                return HttpNotFound();
            }
            return View(delete);
        }
        [HttpPost, ActionName("DeleteFile")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFileConfirmed(int id)
        {
            unitOfWork.FileRepos.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("FileList");
        }

        public ActionResult VideoList()
        {
            var list = unitOfWork.VideoRepos.GetAll();
            return View(list);
        }
        public ActionResult AddVideo()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVideo(Video video)
        {
            if(ModelState.IsValid)
            {
                unitOfWork.VideoRepos.Create(video);
                unitOfWork.Save();
                return RedirectToAction("VideoList");
            }
            return View();
        }

        public ActionResult EditVideo(int id)
        {
            var edit = unitOfWork.VideoRepos.Detail(id);
            if(edit == null)
            {
                return HttpNotFound();
            }
            return View(edit);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditVideo(Video video)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.VideoRepos.Update(video);
                unitOfWork.Save();
                return RedirectToAction("VideoList");
            }
            return View(video);
        }
        public ActionResult VideoDetails(int id)
        {
            var detail = unitOfWork.VideoRepos.Detail(id);
            if (detail == null)
            {
                return HttpNotFound();
            }
            return View(detail);
        }
        public ActionResult DeleteVideo(int id)
        {
            var delete = unitOfWork.VideoRepos.Detail(id);
            if (delete == null)
            {
                return HttpNotFound();
            }
            return View(delete);
        }
        [HttpPost, ActionName("DeleteVideo")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteVideoConfirmed(int id)
        {
            unitOfWork.VideoRepos.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("VideoList");
        }

        public ActionResult TeacherList()
        {
            var list = unitOfWork.TeacherRepos.GetAll();
            return View(list);
        }

        public ActionResult CreateTeacher()
        {
            //ViewBag.PositionId = PositionDropDownList();
            PositionDropDownList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTeacher(HttpPostedFileBase src, Teacher teacher)
        {
            PositionDropDownList();
            if (ModelState.IsValid)
            {
                unitOfWork.TeacherRepos.Create(src, teacher);
                unitOfWork.Save();
                return RedirectToAction("TeacherList");
            }
            return View(teacher);
        }

        public ActionResult EditTeacher(int id)
        {
            
            var edit = unitOfWork.TeacherRepos.Detail(id);
            if (edit == null)
            {
                return HttpNotFound();
            }
            //PositionDropDownList(edit.PositionId);
            // ViewBag.PositionId = PositionDropDownList(edit.PositionId);
            ViewBag.PositionId = new SelectList(unitOfWork.PositionRepos.GetAll(), "PositionId", "Title", edit.PositionId);
            return View(edit);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditTeacher(HttpPostedFileBase src, Teacher edit)
        {
            PositionDropDownList(edit.PositionId);
            //ViewBag.PositionId = PositionDropDownList(edit.PositionId);
            if (ModelState.IsValid)
            {
                unitOfWork.TeacherRepos.Update(src, edit);
                unitOfWork.Save();
                return RedirectToAction("TeacherList");
            }
            return View(edit);
        }

        public ActionResult TeacherDetails(int id)
        {
            var detail = unitOfWork.TeacherRepos.Detail(id);
            if (detail == null)
            {
                return HttpNotFound();
            }
            return View(detail);
        }

        public ActionResult DeleteTeacher(int id)
        {
            var delete = unitOfWork.TeacherRepos.Detail(id);
            if (delete == null)
            {
                return HttpNotFound();
            }
            return View(delete);
        }
        [HttpPost, ActionName("DeleteTeacher")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTeacherConfirmed(int id)
        {
            unitOfWork.TeacherRepos.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("TeacherList");
        }

        public ActionResult PositionList()
        {
            var list = unitOfWork.PositionRepos.GetAll();
            return View(list);
        }

        public ActionResult CreatePosition()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePosition(Position position)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.PositionRepos.Create(position);
                unitOfWork.Save();
                return RedirectToAction("PositionList");
            }
            return View(position);
        }

        public ActionResult EditPosition(int id)
        {
            var edit = unitOfWork.PositionRepos.Detail(id);
            if (edit == null)
            {
                return HttpNotFound();
            }
            return View(edit);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditPosition(Position edit)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.PositionRepos.Update(edit);
                unitOfWork.Save();
                return RedirectToAction("PositionList");
            }
            return View(edit);
        }

        public ActionResult PositionDetails(int id)
        {
            var detail = unitOfWork.PositionRepos.Detail(id);
            if (detail == null)
            {
                return HttpNotFound();
            }
            return View(detail);
        }

        public ActionResult DeletePosition(int id)
        {
            var delete = unitOfWork.PositionRepos.Detail(id);
            if (delete == null)
            {
                return HttpNotFound();
            }
            return View(delete);
        }
        [HttpPost, ActionName("DeletePosition")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePositionConfirmed(int id)
        {
            unitOfWork.PositionRepos.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("PositionList");
        }

        public ActionResult BlogList()
        {
            var list = unitOfWork.BlogRepos.GetAll();
            return View(list);
        }

        public ActionResult CreateBlog()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBlog(HttpPostedFileBase src, Blog blog)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.BlogRepos.Create(src, blog, User.Identity);
                unitOfWork.Save();
                return RedirectToAction("BlogList");
            }
            return View(blog);
        }

        public ActionResult EditBlog(int id)
        {

            var edit = unitOfWork.BlogRepos.Detail(id);
            if (edit == null)
            {
                return HttpNotFound();
            }
            return View(edit);
        }
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditBlog(HttpPostedFileBase src, Blog edit)
        {
            //if (ModelState.IsValid)
            //{
                unitOfWork.BlogRepos.Update(src, edit, User.Identity);
                unitOfWork.Save();
                return RedirectToAction("BlogList");
            //}
            //return View(edit);
        }
        public ActionResult DeleteBlog(int id)
        {
            var delete = unitOfWork.BlogRepos.Detail(id);
            if (delete == null)
            {
                return HttpNotFound();
            }
            return View(delete);
        }
        [HttpPost, ActionName("DeleteBlog")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBlogConfirmed(int id)
        {
            unitOfWork.BlogRepos.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("BlogList");
        }

        private void PositionDropDownList(object select = null)
        {
            var lst = unitOfWork.PositionRepos.GetAll();
            ViewBag.PositionId = new SelectList(lst, "PositionId", "Title", select);
        }
        //public SelectList PositionDropDownList(object select = null)
        //{
        //    var lst = unitOfWork.PositionRepos.GetAll();
        //    var list = new SelectList(lst, "PositionId", "Title", select);
        //    return list;
        //}
        public SelectList CourseDropDownList(object select = null)
        {
            var lst = unitOfWork.Courses.GetAll();
            var list = new SelectList(lst, "CourseId", "Title", select);
            return list;
        }

        private void CategoryDropDownList(object select = null)
        {
            var lst = unitOfWork.Categories.GetAll();
            ViewBag.CategoryId = new SelectList(lst, "CategoryId", "Title", select);
        }
        //public SelectList CategoryDropDownList(object select = null)
        //{
        //    var lst = unitOfWork.Categories.GetAll();
        //    //ViewBag.CategoryId = new SelectList(list, "CategoryId", "Title", select);
        //    var list = new SelectList(lst, "CategoryId", "Title", select);
        //    return list;
        //}



        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}