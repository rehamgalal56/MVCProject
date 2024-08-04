using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVCProject.BLL.Intarfaces;
using MVCProject.BLL.Repositories;
using MVCProject.PL.Helpers;
using MVCProject.PL.ViewModels;
using MVCProject_DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MVCProject.PL.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork/*IEmployeeRepository employeeRepository*/
            , IWebHostEnvironment env
            ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
            _env = env;
            _mapper = mapper;
        }

        public  async Task<IActionResult> Index(string searchInp)
        {
            var employees = Enumerable.Empty<Employee>();
            var employeeRepo = _unitOfWork.Repository<Employee>() as EmployeeRepository;
            if (string.IsNullOrEmpty(searchInp))
                   employees =  await employeeRepo.GetAllAsync();
            else
                employees = employeeRepo.SearchByName(searchInp.ToLower());
            return View(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees));
        }
        //[HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) 
            {
                employeeVM.ImageName =await DocumentSettings.UploadFile(employeeVM.Image,"Images");
                _unitOfWork.Repository<Employee>().Add(_mapper.Map<EmployeeViewModel,Employee>(employeeVM));
                var count =await _unitOfWork.Complete();
                if (count > 0){
                    TempData["Created"] = "Employee Is Created";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(employeeVM);
        }

        //[HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();  //400

            var employee =await _unitOfWork.Repository<Employee>().GetAsync(id.Value);

            if (employee is null)
                return NotFound();    //404
            if(viewName.Equals("Delete",StringComparison.OrdinalIgnoreCase))
                 TempData["ImageName"] = employee.ImageName;
            return View(viewName, _mapper.Map<Employee, EmployeeViewModel>(employee));
        }
        //[HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public  async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(employeeVM);
            try
            {
                employeeVM.ImageName =await DocumentSettings.UploadFile(employeeVM.Image, "Images");
                _unitOfWork.Repository<Employee>().Update(_mapper.Map<EmployeeViewModel, Employee>(employeeVM));
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the employee ");
                return View(employeeVM);
            }
        }
        //[HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM)
        {
            try
            {
                employeeVM.ImageName = TempData["ImageName"] as string;
                _unitOfWork.Repository<Employee>().Delete(_mapper.Map<EmployeeViewModel, Employee>(employeeVM));
                var count =await _unitOfWork.Complete();
                if (count > 0)
                {
                    DocumentSettings.DeleteFile(employeeVM.ImageName,"images");
                    TempData["Deleted"] = "Employee Is Deleted";
                    return RedirectToAction(nameof(Index));
                }
                return View(employeeVM);

            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Detele the employee ");
                return View(employeeVM);
            }
        }

    }
}
