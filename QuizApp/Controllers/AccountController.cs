
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizApp.Data;
using QuizApp.Models.ViewModels;
using QuizApp.Data.Static;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace QuizApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }





        public IActionResult Login() => View(new LoginVM());

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);
            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null)
            {
                //var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                //if (passwordCheck)
                //{
                   
                    if (result.Succeeded)
                    {
                    var currentUser = await _userManager.FindByEmailAsync(loginVM.EmailAddress);

                    // Check if the user is not null
                    if (currentUser != null)
                        {
                            // Retrieve the "Department" claim from the user
                            var departmentClaim = await _userManager.GetClaimsAsync(currentUser)
                                .ContinueWith(claims => claims.Result.FirstOrDefault(c => c.Type == "Department")?.Value);

                            // Use the departmentClaim as needed
                            Console.WriteLine($"Department Claim Value: {departmentClaim}");

                            if (!string.IsNullOrEmpty(departmentClaim))
                            {
                                // Redirect based on the department claim
                                switch (departmentClaim)
                                {
                                    case "Support Department":
                                        return RedirectToAction("Support", "Quiz");
                                    case "Development Department":
                                        return RedirectToAction("Development", "Quiz");
                                    case "Data Analysis Department":
                                        return RedirectToAction("DataAnalysis", "Quiz");
                                        // Add more cases as needed
                                }
                            }
                        }

                        // Redirect to a default page if the department claim is not found
                        return RedirectToAction("Index", "Home");
                    }
                  
                //}
                TempData["Error"] = "Wrong credentials. Please, try again!";
                return View(loginVM);
            }

            TempData["Error"] = "Wrong credentials. Please, try again!";
            return View(loginVM);
        }

        public IActionResult Register()
        {
          var data =  _context.Departments
                    .Select(d => new DepartmentViewModel
                    {
                        Id = d.Id,
                        Name = $"{d.Support} - {d.Development} - {d.DataAnalysis}" // Adjust as needed
                    })
                    .ToList();
            var viewModel = new RegisterVM
            {
                Departments = data
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            var existingUser = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (existingUser != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }
            Console.WriteLine($"ClickedDepartment value before querying the database: {registerVM.ClickedDepartment}");

            var selectedDepartment = await _context.Departments
        .FirstOrDefaultAsync(d => d.Support == registerVM.ClickedDepartment || d.Development == registerVM.ClickedDepartment || d.DataAnalysis == registerVM.ClickedDepartment);

            if (selectedDepartment == null)
            {
                // Handle the case where the selected department is not found in the database
                ModelState.AddModelError(string.Empty, "Invalid department selected.");
                return View(registerVM);
            }


            var newUser = new ApplicationUser
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);


                selectedDepartment.Users.Add(new DepartmentUser { UserId = newUser.Id });
                await _userManager.AddClaimAsync(newUser, new Claim("Department", registerVM.ClickedDepartment));

                // Optionally sign in the user after registration.
                await _signInManager.SignInAsync(newUser, isPersistent: false);

                return View("Login");
            }
            else
            {
                foreach (var error in newUserResponse.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(registerVM);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
      
        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }

    }
}
