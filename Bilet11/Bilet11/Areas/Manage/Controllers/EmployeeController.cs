using Bilet11.DAL;
using Bilet11.Models;
using Bilet11.Utilities;
using Bilet11.Utilities.Enums;
using Bilet11.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Bilet11.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        AppDbContext _context { get; }
        IWebHostEnvironment _env { get; }

        public EmployeeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page = 1)
        {
            PaginateVM<Employee> paginateVM = new PaginateVM<Employee>();
            paginateVM.CurrentPage = page;
            paginateVM.MaxPageCount = (int)Math.Ceiling((decimal)_context.Employees.Count() / 3);
            if (paginateVM.MaxPageCount == 0)
            {
                paginateVM.MaxPageCount = 1;
            }
            if (page < 1 || page > paginateVM.MaxPageCount) return BadRequest();
            paginateVM.Items = _context.Employees.Skip((page-1)* 3).Take(3).Include(e=>e.Position);
            return View(paginateVM);
        }
        public IActionResult Create()
        {
            ViewBag.Positions = new SelectList(_context.Positions.ToList(),nameof(Position.Id),nameof(Position.Name));
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateEmployeeVM createEmployee)
        {
            var image = createEmployee.Image;
            if (image != null)
            {
                string result = image.CheckValidate("image/", 600);
                if (result.Length >0)
                {
                    ModelState.AddModelError("Image", result);
                }
            }
            if (!_context.Positions.Any(p=>p.Id == createEmployee.PositionId))
            {
                ModelState.AddModelError("PositionId", "There is no position in this id!");
            }
            if(!ModelState.IsValid)
            {
                ViewBag.Positions = new SelectList(_context.Positions.ToList(), nameof(Position.Id), nameof(Position.Name));
                return View();
            }
            Employee employee = new Employee()
            {
                Name= createEmployee.Name,
                Surname=createEmployee.Surname,
                Description=createEmployee.Description,
                ImageUrl = createEmployee.Image.SaveFile(_env.WebRootPath, "assets/images"),
                FbLink= createEmployee.FbLink,
                InstagramLink= createEmployee.InstagramLink,
                TwitterLink= createEmployee.TwitterLink,
                LinkedinLink= createEmployee.LinkedinLink,
                PositionId=createEmployee.PositionId
            };
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Employee exist = _context.Employees.Include(e=>e.Position).FirstOrDefault(e=> e.Id == id);
            if (exist is null) return NotFound();
            UpdateEmployeeVM updateEmployee = new UpdateEmployeeVM()
            {
                Name= exist.Name,
                Surname= exist.Surname,
                Description=exist.Description,
                PositionId= exist.PositionId,
                FbLink= exist.FbLink,
                InstagramLink= exist.InstagramLink,
                TwitterLink = exist.TwitterLink,
                LinkedinLink= exist.LinkedinLink,
                ImageUrl = exist.ImageUrl,
            };
            ViewBag.Positions = new SelectList(_context.Positions.ToList(), nameof(Position.Id), nameof(Position.Name));
            ViewBag.Image = exist.ImageUrl;
            return View(updateEmployee);
        }
        [HttpPost]
        public IActionResult Update(int? id,UpdateEmployeeVM updateEmployee)
        {
            if (id is null || id <= 0) return BadRequest();
            var image = updateEmployee.Image;
            string result = image?.CheckValidate("image/", 600);
            if (result?.Length > 0)
            {
                ModelState.AddModelError("Image", result);
            }
            if (!_context.Positions.Any(p => p.Id == updateEmployee.PositionId))
            {
                ModelState.AddModelError("PositionId", "There is no position in this id!");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = new SelectList(_context.Positions.ToList(), nameof(Position.Id), nameof(Position.Name));
                ViewBag.Image = _context.Employees.FirstOrDefault(e=>e.Id == id).ImageUrl;
                return View();
            }
            Employee exist = _context.Employees.Include(e => e.Position).FirstOrDefault(e => e.Id == id);
            if (exist is null) return NotFound();
            if (image != null)
            {
                var newImg = image.SaveFile(_env.WebRootPath, "assets/images");
                exist.ImageUrl.DeleteFile(_env.WebRootPath, "assets/images");
                exist.ImageUrl= newImg;
            }
            exist.Name = updateEmployee.Name;
            exist.Surname= updateEmployee.Surname;
            exist.Description = updateEmployee.Description;
            exist.PositionId= updateEmployee.PositionId;
            exist.FbLink= updateEmployee.FbLink;
            exist.InstagramLink= updateEmployee.InstagramLink;
            exist.TwitterLink= updateEmployee.TwitterLink;
            exist.LinkedinLink= updateEmployee.LinkedinLink;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Employee exist = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (exist is null) return NotFound();
            exist.ImageUrl.DeleteFile(_env.WebRootPath, "assets/images");
            _context.Employees.Remove(exist);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
