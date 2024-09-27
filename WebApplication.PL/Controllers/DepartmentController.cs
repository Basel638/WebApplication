using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using WebApplication.BLL.Interfaces;
using WebApplication.BLL.Repositories;
using WebApplication.DAL.Models;

namespace WebApplication.PL.Controllers
{
    // 2 Relationships
    //	Inhertiance : DepartmentController is Controller
    //	Assocssiation (Composition [Required]): DepartmentController has a DepartmentRepository
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IUnitOfWork unitOfWork, IWebHostEnvironment env) // Ask CLR for creating an object from class impllementing IDepartmentRepository
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var departments =await _unitOfWork.Repository<Department>().GetAll(); 
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
               _unitOfWork.Repository<Department>().Add(department);
                var count = await _unitOfWork.Complete();
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(department);

        }

        // /Department/Details/10
        // /Department/Details      => Nullable Id

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();    // 400

            var department = await _unitOfWork.Repository<Department>().Get(id.Value);

            if (department is null)
                return NotFound();      // 404

            return View(viewName, department);
        }

        // /Department/Edit/10
        // /Department/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            /*
            if(!id.HasValue)
                return BadRequest(); // 400
            var department = _departmentRepo.Get(id.Value);

            if(department is null)
                return NotFound();  // 404

            return View(department);*/

            return await Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id,Department department)
        {

            if(id != department.Id)
                return BadRequest();


            if (!ModelState.IsValid)
                return View(department);


            try
            {
                _unitOfWork.Repository<Department>().Update(department);
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
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the Department");

                return View(department);
            }
        }


        // /Department/Delete/10
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Department department)
        {
            try
            {
                _unitOfWork.Repository<Department>().Delete(department);
             await   _unitOfWork.Complete();
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
