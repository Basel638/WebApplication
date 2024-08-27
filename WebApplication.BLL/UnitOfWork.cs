using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.BLL.Interfaces;
using WebApplication.BLL.Repositories;
using WebApplication.DAL.Data;
using WebApplication.DAL.Models;

namespace WebApplication.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;


        private Hashtable _repositories;
        public UnitOfWork(ApplicationDbContext dbContext)  //ASK CLR for creating Object from 'DbContext'
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }
        public IGenericRepository<T> Repository<T>() where T : ModelBase
        {
            var key = typeof(T).Name;   // Employee


            if(! _repositories.ContainsKey(key))
            {

                if (key == nameof(Employee))
                {
                    var repository = new EmployeeRepo(_dbContext);
                    _repositories.Add(key, repository);
                }

                else
                { 
                var repository = new GenericRepo<T>(_dbContext);
                    _repositories.Add(key, repository);

                }

            }
            return _repositories[key] as IGenericRepository<T>;

        }
        public int Complete()
        => _dbContext.SaveChanges();



        public void Dispose()
            => _dbContext.Dispose();

    }
}
