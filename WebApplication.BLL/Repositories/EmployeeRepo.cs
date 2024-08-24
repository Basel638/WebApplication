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
    public class EmployeeRepo : GenericRepo<Employee>, IEmployeeRepository
    {
       // private readonly ApplicationDbContext _dbContext;

        public EmployeeRepo(ApplicationDbContext dbContext):base(dbContext) // Ask CLR for Creating Object from ApplicationDbContext Class
        {
         //   _dbContext = dbContext;
        }
        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            return _dbContext.Employees.Where(E => E.Address.ToLower() == address.ToLower());
        }
    }
}
 
