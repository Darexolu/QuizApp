using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Full name")]
        public string? FullName { get; set; }
        //public long DepartmentId { get; set; }

        //[ForeignKey("DepartmentId")]
        public Department? Department { get; set; }
        public string? ImageUrl { get; set; }
        
        public ICollection<DepartmentUser> Departments { get; set; } = new List<DepartmentUser>();
    }
}
