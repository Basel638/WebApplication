using Microsoft.AspNetCore.Mvc;
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
        private readonly IDepartmentRepository _departmentRepo;	//NULL

        public DepartmentController(IDepartmentRepository departmentRepo) // Ask CLR for creating an object from class impllementing IDepartmentRepository
        {
            _departmentRepo = departmentRepo;

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
    }
}
