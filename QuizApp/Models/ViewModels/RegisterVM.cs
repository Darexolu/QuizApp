using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace QuizApp.Models.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "User Name is required")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        [ValidateNever]

        [Display(Name = "Departments")]
        public List<DepartmentViewModel> Departments  { get; set; }
        [Display(Name = "Selected Departments")]
        [ValidateNever]
        public List<int> SelectedDepartmentIds { get; set; }
        public string ClickedDepartment { get; set; }
    }
}
