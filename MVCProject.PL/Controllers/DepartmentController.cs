using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVCProject.BLL.Intarfaces;
using MVCProject.BLL.Repositories;
using MVCProject_DAL.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MVCProject.PL.Controllers
{
	[Authorize]
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

		public async Task<IActionResult> Index(string searchInp)
		{
			var departments = Enumerable.Empty<Department>();
			if (string.IsNullOrEmpty(searchInp))
				departments = await _unitOfWork.Repository<Department>().GetAllAsync();
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
		public async Task<IActionResult> Create(Department department)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Repository<Department>().Add(department);

				var count = await _unitOfWork.Complete();

				if (count > 0)
				{
					TempData["Created"] = "Department Is Created";
					return RedirectToAction(nameof(Index));
				}
			}
			return View(department);
		}

		[HttpGet]
		public async Task<IActionResult> Details(int? id, string viewName = "Details")
		{
			if (!id.HasValue)
				return BadRequest();  //400

			var department = await _unitOfWork.Repository<Department>().GetAsync(id.Value);

			if (department is null)
				return NotFound();    //404

			return View(viewName, department);
		}
		[HttpGet]
		public async Task<IActionResult> Edit(int? id)
		{
			return await Details(id, "Edit");
		}
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([FromRoute] int id, Department department)
		{
			if (id != department.Id)
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
				if (_env.IsDevelopment())
					ModelState.AddModelError(string.Empty, ex.Message);
				else
					ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the Department ");
				return View(department);
			}
		}
		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			return await Details(id, "Delete");
		}
		[HttpPost]
		public async Task<IActionResult> Delete(Department department)
		{
			try
			{
				_unitOfWork.Repository<Department>().Delete(department);
				await _unitOfWork.Complete();
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
