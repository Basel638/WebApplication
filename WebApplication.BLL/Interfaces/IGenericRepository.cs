using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DAL.Models;

namespace WebApplication.BLL.Interfaces
{
    public interface IGenericRepository<T>where T : ModelBase // ModelBase Or any class ingerit from modelbase
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> Get(int? id);

        void Add(T entity);

		void Update(T entity);

		void Delete(T entity);
    }
}
