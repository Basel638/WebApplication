﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DAL.Models;

namespace WebApplication.DAL.Data.Configurations
{
	internal class DepartmentConfigurations : IEntityTypeConfiguration<Department>
	{
		public void Configure(EntityTypeBuilder<Department> builder)
		{
			//Fluent APIs for Department Domain

			builder.Property(D=>D.Id).UseIdentityColumn(10,10);
			builder.Property(D=>D.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired();	
			builder.Property(D=>D.Code).HasColumnType("varchar").HasMaxLength(50).IsRequired();	 
		}
	}
}
