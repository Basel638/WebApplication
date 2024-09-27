﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DAL.Models;

namespace WebApplication.BLL.Interfaces
{
    // Property Signature
    public interface IUnitOfWork : IAsyncDisposable
    {

        IGenericRepository<T> Repository<T>() where T : ModelBase;
        Task<int> Complete();
    }
}
