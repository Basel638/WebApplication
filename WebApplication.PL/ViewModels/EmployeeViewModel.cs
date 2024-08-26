﻿using System.ComponentModel.DataAnnotations;
using System;
using WebApplication.DAL.Models;

namespace WebApplication.PL.ViewModels
{
	public class EmployeeViewModel
	{
        public int Id { get; set; }

        [Required]
		[MaxLength(50, ErrorMessage = "Max Length of Name is 50 Chars")]
		[MinLength(5, ErrorMessage = "Min Length of Name is 5 Chars")]
		public string Name { get; set; }

		[Range(22, 30)]
		public int? Age { get; set; }

		[RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$"
			, ErrorMessage = "Address must be like 123-Street-City-Country")]
		public string Address { get; set; }

		[DataType(DataType.Currency)]
		public decimal Salary { get; set; }

		[Display(Name = "Is Active")]
		public bool IsActive { get; set; }

		[EmailAddress]
		public string Email { get; set; }
		[Display(Name = "Phone Number")]
		[Phone]
		public string PhoneNumber { get; set; }

		[Display(Name = "Hiring Date")]
		public DateTime HiringDate { get; set; }


		public DateTime CreationDate { get; set; } = DateTime.Now;

		public bool IsDeleted { get; set; } = false;


		public Gender Gender { get; set; }

		public EmpType EmployeeType { get; set; }


		public int? DepartmentId { get; set; }   // Foreign Key Column

		//[InverseProperty(nameof(Models.Department.Employees))]
		// Navigational Property ONE
		public Department Department { get; set; }

	}
}