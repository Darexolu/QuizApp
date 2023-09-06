using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }    
        //public long DepartmentId { get; set; }

        //[ForeignKey("DepartmentId")]
        //public Department? Department { get; set; }
        public string ImageUrl { get; set; }    
    }
}
