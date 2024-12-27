using Doctors_Web_Forum.BLL.IServices;
using Doctors_Web_Forum.BLL.Services;
using Doctors_Web_Forum.DAL.Data;
using Doctors_Web_Forum.DAL.Models;
using Doctors_Web_Forum.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Add Service DbContext
builder.Services.AddDbContext<DataDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSQL")));

// add scoped
builder.Services.AddScoped<ITopicService, TopicService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<DataDBContext>().AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 1;
    options.User.RequireUniqueEmail = true;
});


var app = builder.Build();

// config Areas Admin

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// allow access to static resources in wwwroot
app.UseStaticFiles();
// allow navigation
app.UseRouting();
// allow authentication
app.UseAuthentication();
// allow authorization
app.UseAuthorization();


// default path when accessing the project
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
