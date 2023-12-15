using Microsoft.AspNetCore.Identity;
using QuizApp.Data.Static;
using QuizApp.Models;

namespace QuizApp.Data
{
    public class AppDbInitializer
    {
        public static void SeedData(AppDbContext dbContext)
        {
            if (!dbContext.Departments.Any())
            {
                // Add initial departments here
                var initialDepartments = new List<Department>
            {
                new Department { Support = "Support Department", Development = "Development Department", DataAnalysis = "Data Analysis Department" },
                // Add more departments as needed
            };

                dbContext.Departments.AddRange(initialDepartments);
                dbContext.SaveChanges();
            }
        }
            public static  async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var adminUserName = "admin@app.com";
                var adminUser = await userManager.FindByNameAsync(adminUserName);

                if (adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        Email = adminUserName,
                        UserName = adminUserName,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Adminapp1*");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }


                var appUserName = "user@app.com";
                var appUser = await userManager.FindByNameAsync(appUserName);
                if (appUser == null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        Email = appUserName,
                        UserName = appUserName,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppUser, "Userapp1*");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
