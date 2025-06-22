using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizWebApp;
using QuizWebApp.Models;


var builder = WebApplication.CreateBuilder(args);

// Po³¹czenie do bazy danych
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// KONFIGURACJA Identity z ApplicationUser
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; 
})
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();


// Routing g³ówny
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Quiz}/{action=MyQuizzes}/{id?}");


app.MapRazorPages(); // dla Identity (register/login)

app.Run();

