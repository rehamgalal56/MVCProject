using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVCProject.BLL.Intarfaces;
using MVCProject.BLL.Repositories;
using MVCProject_DAL.Models;
using System;
using System.Linq;

namespace MVCProject.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IUnitOfWork unitOfWork/*IDepartmentRepository departmentRepository*/, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            //_departmentRepository = departmentRepository;
            _env = env;
        }

        public IActionResult Index(string searchInp)
        {
            var departments = Enumerable.Empty<Department>();
            if (string.IsNullOrEmpty(searchInp))
                departments = _unitOfWork.Repository<Department>().GetAll();
            else
                departments = _unitOfWork.Repository<Department>().SearchByName(searchInp.ToLower());
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
            if(ModelState.IsValid)
            {
                _unitOfWork.Repository<Department>().Add(department);

                var count = _unitOfWork.Complete();
                  
                if(count > 0) {
                    TempData["Created"] = "Department Is Created";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(department);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName="Details") 
        {
            if (!id.HasValue)
                return BadRequest();  //400

            var department = _unitOfWork.Repository<Department>().Get(id.Value);

            if(department is null)
                return NotFound();    //404

            return View(viewName,department);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
          return Details(id, "Edit");
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id,Department department)
        {
            if (id != department.Id)
                return BadRequest();

            if (!ModelState.IsValid) 
                return View(department);
            try
            {
                _unitOfWork.Repository<Department>().Update(department);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the Department ");
                return View(department);
            }
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public IActionResult Delete(Department department)
        {
            try
            {
                _unitOfWork.Repository<Department>().Delete(department);
                _unitOfWork.Complete();
                TempData["Deleted"] = "Department Is Deleted";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Detele the Department ");
                return View(department);
            }
        }

    }
}
