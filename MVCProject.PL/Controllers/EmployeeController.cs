using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVCProject.BLL.Intarfaces;
using MVCProject.BLL.Repositories;
using MVCProject_DAL.Models;
using System;
using System.ComponentModel;
using System.Linq;

namespace MVCProject.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IUnitOfWork unitOfWork/*IEmployeeRepository employeeRepository*/, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
            _env = env;
        }

        public IActionResult Index(string searchInp)
        {
            var employees = Enumerable.Empty<Employee>();
            var employeeRepo = _unitOfWork.Repository<Employee>() as EmployeeRepository;
            if (string.IsNullOrEmpty(searchInp))
                   employees = employeeRepo.GetAll();
            else
                employees = employeeRepo.SearchByName(searchInp.ToLower());
            return View(employees);
        }
        //[HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Repository<Employee>().Add(employee);
                var count = _unitOfWork.Complete();
                if (count > 0){
                    TempData["Created"] = "Employee Is Created";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(employee);
        }

        //[HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();  //400

            var employee = _unitOfWork.Repository<Employee>().Get(id.Value);

            if (employee is null)
                return NotFound();    //404

            return View(viewName, employee);
        }
        //[HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(employee);
            try
            {
                _unitOfWork.Repository<Employee>().Update(employee);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the employee ");
                return View(employee);
            }
        }
        //[HttpGet]
        public IActionResult Delete(int id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            try
            {
                _unitOfWork.Repository<Employee>().Delete(employee);
                _unitOfWork.Complete();
                TempData["Deleted"] = "Employee Is Deleted";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Detele the employee ");
                return View(employee);
            }
        }

    }
}
