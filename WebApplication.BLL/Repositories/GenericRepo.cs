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
    public class GenericRepo<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly ApplicationDbContext _dbContext; // NULL

        public GenericRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public  void Add(T entity)
        {
            _dbContext.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Remove(entity);
        }

        public async Task<T> Get(int? id)
        {
            // Local => Memory of Application
            ///var T = _dbContext.T.Local.Where(D => D.Id == id).FirstOrDefault();
            ///
            ///if (T == null)
            ///	T = _dbContext.T.Where(D => D.Id == id).FirstOrDefault();
            ///return T;

            //return _dbContext.T.Find(id);

            return  await _dbContext.FindAsync<T>(id); // EF Core 3.1 New Feature

            //return (T)_dbContext.Employees.Where(E => E.Id == id).Include(E => E.Department);     // Error غريب 
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            if (typeof(T) == typeof(Employee))
                return (IEnumerable<T>)await _dbContext.Employees.Include(E => E.Department).AsNoTracking().ToListAsync();
            else
                return await _dbContext.Set<T>().AsNoTracking().ToListAsync();

        }

        public void Update(T entity)
        {
            _dbContext.Update(entity);
        }
    }
}
