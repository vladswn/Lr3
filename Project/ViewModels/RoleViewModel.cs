using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Display(Name = "RoleName")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}