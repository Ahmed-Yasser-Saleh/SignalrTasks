using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using signalrTask.Context;
using signalrTask.Hubs;
using signalrTask.Models;

namespace signalrTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<MyContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("app")));
            builder.Services.AddSignalR(); 
            builder.Services.AddSingleton<displayEmpHub>();
            builder.Services.AddIdentity<Employee, IdentityRole>
            (options =>
            {
                options.Password.RequireDigit = true; // Must contain at least one digit
                options.Password.RequiredLength = 3;  // Minimum length of 3 characters
                options.Password.RequireNonAlphanumeric = false; //not Must contain at least one special character
                options.Password.RequireUppercase = false; // not Must contain at least one uppercase letter
            }).AddEntityFrameworkStores<MyContext>().AddDefaultTokenProviders();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Employee/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapHub<displayEmpHub>("/Add-Employee");
            app.MapHub<MoveDivHub>("/Move-Div");
            app.MapHub<ChatHub>("/Chat");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Register}");

            app.Run();
        }
    }
}
