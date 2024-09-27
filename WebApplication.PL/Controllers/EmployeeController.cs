using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.BLL.Interfaces;
using WebApplication.BLL.Repositories;
using WebApplication.DAL.Models;
using WebApplication.PL.Helpers;
using WebApplication.PL.ViewModels;

namespace WebApplication.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly IEmployeeRepository _employeeRepo; //NULL
        //private readonly IDepartmentRepository _departmentRepo;
        private readonly IWebHostEnvironment _env;

        public EmployeeController
            ( /*IEmployeeRepository employeeRepo*/ /*IDepartmentRepository departmentRepo,*/  // Ask CLR for creating an object from class impllementing IEmployeeRepository 
            IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
            //_employeeRepo = employeeRepo;
            //_departmentRepo = departmentRepo;
        }
        public async Task<IActionResult> Index(string searchInp)
        {
            var Employees = Enumerable.Empty<Employee>();

            var employeeRepo = _unitOfWork.Repository<Employee>() as EmployeeRepo;
            if (!string.IsNullOrEmpty(searchInp))
            {
                Employees = employeeRepo.GetEmployeesByName(searchInp.ToLower());
            }
            else
            {

                // Binding Through Views's Dictionary : Transfer Data from Action to View [One Way]


                // 1. ViewData
                ViewData["Message"] = "Hello ViewData";

                // 2. ViewBag
                ViewBag.Message = "Hello ViewBag";


                Employees = await employeeRepo.GetAll();
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
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                 employeeVM.ImageName = await  DocumentSettings.UploadFile(employeeVM.Image, "images");


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

                _unitOfWork.Repository<Employee>().Add(mappedEmp);

                /// Core Benefit Of UnitOfWork 
                // 2. Update Department    (Draft Example)
                // _unitOfWork.DepartmentRepository.Update(department);

                // 3. Delete Project    (Draft Example)
                // _unitOfWork.ProjectRepository.Delete(Project);



                // 3. TempData
                var count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created Successfully";
                }

                else
                    TempData["Message"] = "An Error Has Ocurred, Employee Not Created :(";

                return RedirectToAction(nameof(Index));

            }
            return View(employeeVM);

        }

        // /Employee/Details/10
        // /Employee/Details      => Nullable Id

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();    // 400

            var employee =await _unitOfWork.Repository<Employee>().Get(id.Value);

            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);
            if (employee is null)
                return NotFound();      // 404


            if(viewName.Equals("Delete",StringComparison.OrdinalIgnoreCase))
            TempData["ImageName"] = employee.ImageName;


			return View(viewName, mappedEmp);
        }

        // /Employee/Edit/10
        // /Employee/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            /*
            if(!id.HasValue)
                return BadRequest(); // 400
            var Employee = _EmployeeRepo.Get(id.Value);

            if(Employee is null)
                return NotFound();  // 404

            return View(Employee);*/
            //ViewData["Departments"] = _departmentRepo.GetAll();

            return await Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {

            if (id != employeeVM.Id)
                return BadRequest();


            if (!ModelState.IsValid)
                return View(employeeVM);


            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _unitOfWork.Repository<Employee>().Update(mappedEmp);
               await _unitOfWork.Complete();
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
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM)
        {
            try
            {
                employeeVM.ImageName = TempData["ImageName"] as string;
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.Repository<Employee>().Delete(mappedEmp);
                var count = await _unitOfWork.Complete();

                if (count > 0)
                {
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "images");
                    return RedirectToAction(nameof(Index));
                }
				return View(employeeVM);

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
