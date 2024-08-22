﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DAL.Models;

namespace WebApplication.BLL.Interfaces
{
	internal interface IDepartmentRepository
	{
		IEnumerable<Department> GetAll();

		Department Get(int id);

		int Add(Department entity);

		int Update(Department entity);

		int Delete(Department entity);
	}
}