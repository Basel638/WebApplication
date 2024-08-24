using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using WebApplication.BLL.Interfaces;
using WebApplication.DAL.Models;

namespace WebApplication.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo; //NULL
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IEmployeeRepository employeeRepo, IWebHostEnvironment env) // Ask CLR for creating an object from class impllementing IEmployeeRepository
        {
            _employeeRepo = employeeRepo;
            _env = env;
        }
        public IActionResult Index()
        {
            var Employees = _employeeRepo.GetAll();
            return View(Employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var count = _employeeRepo.Add(employee);
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(employee);

        }

        // /Employee/Details/10
        // /Employee/Details      => Nullable Id

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();    // 400

            var employee = _employeeRepo.Get(id.Value);

            if (employee is null)
                return NotFound();      // 404

            return View(viewName, employee);
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

            return Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee employee)
        {

            if (id != employee.Id)
                return BadRequest();


            if (!ModelState.IsValid)
                return View(employee);


            try
            {
                _employeeRepo.Update(employee);
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

                return View(employee);
            }
        }


        // /Employee/Delete/10
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }


        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            try
            {
                _employeeRepo.Delete(employee);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the Employee");

                return View(employee);
            }
        }
    }

}
