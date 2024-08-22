using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.BLL.Interfaces;
using WebApplication.DAL.Data;
using WebApplication.DAL.Models;

namespace WebApplication.BLL.Repositories
{
	internal class DepartmentRepo : IDepartmentRepository
	{
		
		private readonly ApplicationDbContext _dbContext; // NULL
		
		public DepartmentRepo(ApplicationDbContext dbContext) // Ask CLR for creating object from "ApplicationDbContext"
		{
			_dbContext = dbContext;
		}

		public int Add(Department entity)
		{
			_dbContext.Departments.Add(entity);
			return _dbContext.SaveChanges();
		}

		public int Delete(Department entity)
		{
			_dbContext.Departments.Remove(entity);
			return _dbContext.SaveChanges();
		}

		public Department Get(int id)
		{
			// Local => Memory of Application
			///var department = _dbContext.Departments.Local.Where(D => D.Id == id).FirstOrDefault();
			///
			///if (department == null)
			///	department = _dbContext.Departments.Where(D => D.Id == id).FirstOrDefault();
			///return department;

			//return _dbContext.Departments.Find(id);
			return _dbContext.Find<Department>(id);	// EF Core 3.1 New Feature

		}

		public IEnumerable<Department> GetAll()
		{
			return _dbContext.Departments.AsNoTracking().ToList();	
		}

		public int Update(Department entity)
		{
			_dbContext.Departments.Update(entity);
			return _dbContext.SaveChanges();
		}
	}
}
