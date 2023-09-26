using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using Microsoft.AspNetCore.Identity;
using QuizApp.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using QuizApp.Repositories.IRepository;
using QuizApp.Repositories;
using QuizApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IQuizRepository, QuizRepository>();

builder.Services.AddScoped<IQuestionService, QuestionService>();


builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(5); // Set the session timeout
    options.Cookie.HttpOnly = true; // Ensure cookies are HTTP-only
    options.Cookie.IsEssential = true; // Make the session cookie essential
});
builder.Services.AddAuthentication("password")
    .AddCookie("password", options =>
    {
        options.Cookie.Name = "password";
        options.LoginPath = "/Quiz/Login"; // Replace with the actual controller and action for login
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;
app.UseAuthorization();
app.MapRazorPages();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
