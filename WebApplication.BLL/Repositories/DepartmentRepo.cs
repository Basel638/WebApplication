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
	public class DepartmentRepo : GenericRepo<Department>,IDepartmentRepository
	{
        public DepartmentRepo(ApplicationDbContext dbContext):base(dbContext)
        {
            
        }

    }
}
