﻿using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.BLL.Interfaces;
using WebApplication.DAL.Models;
using WebApplication.PL.ViewModels;

namespace WebApplication.PL.Controllers
{
	public class EmployeeController : Controller
	{
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepo; //NULL
        //private readonly IDepartmentRepository _departmentRepo;
        private readonly IWebHostEnvironment _env;

		public EmployeeController(IMapper mapper,IEmployeeRepository employeeRepo,/*IDepartmentRepository departmentRepo,*/ IWebHostEnvironment env) // Ask CLR for creating an object from class impllementing IEmployeeRepository
		{
            _mapper = mapper;
            _employeeRepo = employeeRepo;
            //_departmentRepo = departmentRepo;
            _env = env;
		}
		public IActionResult Index(string searchInp)
		{
			var Employees= Enumerable.Empty<Employee>();
			if (!string.IsNullOrEmpty(searchInp))
			{
				 Employees = _employeeRepo.GetEmployeesByName(searchInp.ToLower());
			}
			else
			{

			// Binding Through Views's Dictionary : Transfer Data from Action to View [One Way]


			// 1. ViewData
			ViewData["Message"] = "Hello ViewData";

			// 2. ViewBag
			ViewBag.Message = "Hello ViewBag";


			 Employees = _employeeRepo.GetAll();
			}
			var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Employees);

			return View(mappedEmps);
		}
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}


		[HttpPost]
		public IActionResult Create(EmployeeViewModel employeeVM)
		{
			if (ModelState.IsValid) // Server Side Validation
			{
				// Manual Mapping
				///var mappedEmp = new Employee()
				///{
				///	Name = employeeVM.Name,
				///	Age = employeeVM.Age,
				///	Address = employeeVM.Address,
				///	Salary = employeeVM.Salary,
				///	Email = employeeVM.Email,
				///	PhoneNumber = employeeVM.PhoneNumber,
				///	IsActive = employeeVM.IsActive,
				///	HiringDate = employeeVM.HiringDate
				///
				///};

				var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

				var count = _employeeRepo.Add(mappedEmp);


				// 3. TempData

				if (count > 0)
					TempData["Message"] = "Employee is Created Successfully";

				else
					TempData["Message"] = "An Error Has Ocurred, Employee Not Created :(";

					return RedirectToAction(nameof(Index));

			}
			return View(employeeVM);

		}

		// /Employee/Details/10
		// /Employee/Details      => Nullable Id

		[HttpGet]
		public IActionResult Details(int? id, string viewName = "Details")
		{
			if (id is null)
				return BadRequest();    // 400

			var employee = _employeeRepo.Get(id.Value);

			var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);
			if (employee is null)
				return NotFound();      // 404

			return View(viewName, mappedEmp);
		}

		// /Employee/Edit/10
		// /Employee/Edit
		[HttpGet]
		public IActionResult Edit(int? id)
		{
            /*
            if(!id.HasValue)
                return BadRequest(); // 400
            var Employee = _EmployeeRepo.Get(id.Value);

            if(Employee is null)
                return NotFound();  // 404

            return View(Employee);*/
            //ViewData["Departments"] = _departmentRepo.GetAll();

            return Details(id, "Edit");
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit([FromRoute] int id, EmployeeViewModel employeeVM)
		{

			if (id != employeeVM.Id)
				return BadRequest();


			if (!ModelState.IsValid)
				return View(employeeVM);


			try
			{
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _employeeRepo.Update(mappedEmp);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				// 1. Log Exception
				// 2.  Friendly Message

				if (_env.IsDevelopment())
					ModelState.AddModelError(string.Empty, ex.Message);
				else
					ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the Employee");

				return View(employeeVM);
			}
		}


		// /Employee/Delete/10
		public IActionResult Delete(int? id)
		{
			return Details(id, "Delete");
		}


		[HttpPost]
		public IActionResult Delete(EmployeeViewModel employeeVM)
		{
			try
			{
				var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
				_employeeRepo.Delete(mappedEmp);

				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				if (_env.IsDevelopment())
					ModelState.AddModelError(string.Empty, ex.Message);
				else
					ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the Employee");

				return View(employeeVM);
			}
		}
	}

}
