using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using WebApplication.BLL.Interfaces;
using WebApplication.BLL.Repositories;
using WebApplication.DAL.Models;

namespace WebApplication.PL.Controllers
{
    // 2 Relationships
    //	Inhertiance : DepartmentController is Controller
    //	Assocssiation (Composition [Required]): DepartmentController has a DepartmentRepository
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepo; //NULL
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IDepartmentRepository departmentRepo, IWebHostEnvironment env) // Ask CLR for creating an object from class impllementing IDepartmentRepository
        {
            _departmentRepo = departmentRepo;
            _env = env;
        }
        public IActionResult Index()
        {
            var departments = _departmentRepo.GetAll(); 
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var count = _departmentRepo.Add(department);
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(department);

        }

        // /Department/Details/10
        // /Department/Details      => Nullable Id

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();    // 400

            var department = _departmentRepo.Get(id.Value);

            if (department is null)
                return NotFound();      // 404

            return View(viewName, department);
        }

        // /Department/Edit/10
        // /Department/Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            /*
            if(!id.HasValue)
                return BadRequest(); // 400
            var department = _departmentRepo.Get(id.Value);

            if(department is null)
                return NotFound();  // 404

            return View(department);*/

            return Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id,Department department)
        {

            if(id != department.Id)
                return BadRequest();


            if (!ModelState.IsValid)
                return View(department);


            try
            {
                _departmentRepo.Update(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2.  Friendly Message

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the Department");

                return View(department);
            }
        }


        // /Department/Delete/10
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }


        [HttpPost]
        public IActionResult Delete(Department department)
        {
            try
            {
                _departmentRepo.Delete(department);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the Department");

                return View(department);
            }
        }
    }
}
