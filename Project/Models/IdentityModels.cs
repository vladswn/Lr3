using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity>
         GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager
                .CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Display(Name = "Прізвище")]
        public string Surname { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }

    public class ApplicationRole:IdentityRole
    {
        public ApplicationRole() : base() { }
        [Display(Name = "Опис")]
        public string Description { get; set; }
        public ApplicationRole(string name) : base(name) { }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }


        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Task>().HasMany(c => c.Files)
        //   .WithMany(s => s.Tasks)
        //   .Map(t => t.MapLeftKey("TaskId")
        //   .MapRightKey("FileId")
        //   .ToTable("TaskFiles"));
        //}

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Course>().HasMany(c => c.Users)
        //   .WithMany(s => s.Courses)
        //   .Map(t => t.MapLeftKey("CourseId")
        //   .MapRightKey("UserId")
        //   .ToTable("UsersCourse"));
        //}
        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }


}