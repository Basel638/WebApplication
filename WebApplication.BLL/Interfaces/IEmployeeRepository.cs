using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DAL.Models;

namespace WebApplication.BLL.Interfaces
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {
      IQueryable<Employee> GetEmployeesByAddress(string address);
      IQueryable<Employee> GetEmployeesByName(string name);

    }
}
