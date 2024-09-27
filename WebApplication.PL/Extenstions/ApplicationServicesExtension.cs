using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using WebApplication.BLL;
using WebApplication.BLL.Interfaces;
using WebApplication.BLL.Repositories;
using WebApplication.DAL.Data;
using WebApplication.DAL.Models;
using WebApplication.PL.Helpers;
using WebApplication.PL.Services.EmailSender;

namespace WebApplication.PL.Extenstions
{
	public static class ApplicationServicesExtension
	{
		public static void AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			//services.AddScoped<IDepartmentRepository, DepartmentRepo>();
			//services.AddScoped<IEmployeeRepository, EmployeeRepo>();
			services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));


			//services.AddScoped<UserManager<ApplicationUser>>();
			//services.AddScoped<SignInManager<ApplicationUser>>();
			//services.AddScoped<RoleManager<IdentityRole>>();
			//services.AddAuthentication();

			services.AddIdentity<ApplicationUser, IdentityRole>()
				 .AddEntityFrameworkStores<ApplicationDbContext>()
				 .AddDefaultTokenProviders();

			// Default Schema {Idenetity.Application}
			services.AddAuthentication("Hamada");


			// Configuration of current default schema of Your Application
			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Account/SignIn";
				//options.ExpireTimeSpan = TimeSpan.FromDays(1);
				options.AccessDeniedPath = "/Home/Error";


			});

					/*
			services.AddAuthentication(options =>
			{
				//options.DefaultAuthenticateScheme = "Hamada";
				//options.DefaultChallengeScheme = "Hamada";
				
			})
				.AddCookie("Hamada", options =>
				{

					options.LoginPath = "/Account/SignIn";
					//options.ExpireTimeSpan = TimeSpan.FromDays(1);
					options.AccessDeniedPath = "/Home/Error";

				});
*/

			services.AddTransient<IEmailSender, EmailSender>();	

		}
	}
}
