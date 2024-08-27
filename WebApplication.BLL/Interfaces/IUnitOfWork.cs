using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DAL.Models;

namespace WebApplication.BLL.Interfaces
{
    // Property Signature
    public interface IUnitOfWork : IDisposable
    {

        IGenericRepository<T> Repository<T>() where T : ModelBase;
        int Complete();
    }
}
