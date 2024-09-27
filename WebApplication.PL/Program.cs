using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication.DAL.Data;
using WebApplication.PL.Extenstions;

namespace WebApplication.PL
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var WebApplicationBuilder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);


			#region Configure Services
			WebApplicationBuilder.Services.AddControllersWithViews();
			//WebApplicationBuilder.Services.AddTransient<ApplicationDbContext>();	// per request
			//WebApplicationBuilder.Services.AddSingleton<ApplicationDbContext>();	//Per session

			/*
			WebApplicationBuilder.Services.AddScoped<ApplicationDbContext>();     // just on per request
			WebApplicationBuilder.Services.AddScoped<DbContextOptions<ApplicationDbContext>>();
			*/
			WebApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(WebApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			}); //Default Scoped (ApplicationDbContext,DbContextOptions)


			WebApplicationBuilder.Services.AddApplicationServices();  // Extension Method

			#endregion

			var app = WebApplicationBuilder.Build();

			#region Configure Kestrel MiddleWares



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

			app.Run(); // Application is Ready for Requests
		}


	}
}
