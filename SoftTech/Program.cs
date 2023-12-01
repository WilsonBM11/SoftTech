using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftTech.Models.Domain;
using SoftTech.Repositories.Abstract;
using SoftTech.Repositories.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataBaseSecurityContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));
//For Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DataBaseSecurityContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<IUserAdministrationService, UserAdministrationService>(); 

builder.Services.ConfigureApplicationCookie(op => op.LoginPath = "/UserAuthentication/Authentication");

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserAuthentication}/{action=Authentication}/{id?}");

app.Run();
