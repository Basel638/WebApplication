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

        public int Add(T entity)
        {
            _dbContext.Add(entity);
            return _dbContext.SaveChanges();
        }

        public int Delete(T entity)
        {
            _dbContext.Remove(entity);
            return _dbContext.SaveChanges();
        }

        public T Get(int id)
        {
            // Local => Memory of Application
            ///var T = _dbContext.T.Local.Where(D => D.Id == id).FirstOrDefault();
            ///
            ///if (T == null)
            ///	T = _dbContext.T.Where(D => D.Id == id).FirstOrDefault();
            ///return T;

            //return _dbContext.T.Find(id);
            return _dbContext.Find<T>(id); // EF Core 3.1 New Feature

        }

        public IEnumerable<T> GetAll()
        {
            return _dbContext.Set<T>().AsNoTracking().ToList();
        }

        public int Update(T entity)
        {
            _dbContext.Update(entity);
            return _dbContext.SaveChanges();
        }
    }
}
