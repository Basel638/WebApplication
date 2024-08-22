using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DAL.Data.Configurations;
using WebApplication.DAL.Models;

namespace WebApplication.DAL.Data
{
	internal class ApplicationDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

			=> optionsBuilder.UseSqlServer("Server = .; Database = MVCApplication; Trusted_Connection = True;");

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//modelBuilder.ApplyConfiguration<Department>(new DepartmentConfigurations());
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}
		public DbSet<Department> Departments { get; set; }
    }
}
