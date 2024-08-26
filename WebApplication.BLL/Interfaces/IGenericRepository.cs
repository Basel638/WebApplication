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
        IEnumerable<T> GetAll();

        T Get(int? id);

        int Add(T entity);

        int Update(T entity);

        int Delete(T entity);
    }
}
