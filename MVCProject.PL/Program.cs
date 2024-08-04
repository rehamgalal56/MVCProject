using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVCProject.PL.Extensions;
using MVCProject_DAL.Data;
using MVCProject_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCProject.PL
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var webApplicationBuilder = WebApplication.CreateBuilder(args);

			#region Configure Services
			webApplicationBuilder.Services.AddControllersWithViews();
			webApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});
			webApplicationBuilder.Services.AddApplicationServices();
			webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
			{
				Options.Password.RequiredUniqueChars = 2;
				Options.Password.RequireUppercase = true;
				Options.Password.RequireLowercase = true;
				Options.Password.RequireUppercase = true;
				Options.Password.RequireDigit = true;
				Options.Password.RequireNonAlphanumeric = true;
				Options.Password.RequiredLength = 5;

				Options.Lockout.AllowedForNewUsers = true;
				Options.Lockout.MaxFailedAccessAttempts = 5;
				Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

				Options.User.RequireUniqueEmail = true;



			})
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			webApplicationBuilder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Account/SignIn";
				options.ExpireTimeSpan = TimeSpan.FromDays(5);
				options.AccessDeniedPath = "/Home/Error";

			});
			#endregion

			var app = webApplicationBuilder.Build();

			#region Configure Kestrel Middl

			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
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

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
			#endregion

			app.Run();
		}

	}
}
