using Bilet11.DAL;
using Bilet11.Models;
using Bilet11.Utilities.Enums;
using Bilet11.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bilet11.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class PositionController : Controller
    {
        AppDbContext _context { get; }

        public PositionController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page =1)
        {
            PaginateVM<Position> paginateVM = new PaginateVM<Position>();
            paginateVM.CurrentPage = page;
            paginateVM.MaxPageCount = (int)Math.Ceiling((decimal)_context.Positions.Count() / 3);
            if (paginateVM.MaxPageCount == 0)
            {
                paginateVM.MaxPageCount = 1;
            }
            if (page < 1 || page > paginateVM.MaxPageCount) return BadRequest();
            paginateVM.Items = _context.Positions.Skip((page - 1) * 3).Take(3);
            return View(paginateVM);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Position createPosition)
        {
            if (_context.Positions.Any(p => p.Name == createPosition.Name))
            {
                ModelState.AddModelError("Name", "This position already exists!");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            _context.Positions.Add(createPosition);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Position exist = _context.Positions.FirstOrDefault(p => p.Id == id);
            if (exist is null) return NotFound();
            return View(exist);
        }
        [HttpPost]
        public IActionResult Update(int? id, Position updatePosition)
        {
            if (id is null || id <= 0) return BadRequest();
            if (_context.Positions.Any(p => p.Id != updatePosition.Id && p.Name == updatePosition.Name))
            {
                ModelState.AddModelError("Name", "This position already exists!");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            Position exist = _context.Positions.FirstOrDefault(e => e.Id == id);
            if (exist is null) return NotFound();
            exist.Name = updatePosition.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Position exist = _context.Positions.FirstOrDefault(e => e.Id == id);
            if (exist is null) return NotFound();
            _context.Positions.Remove(exist);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
