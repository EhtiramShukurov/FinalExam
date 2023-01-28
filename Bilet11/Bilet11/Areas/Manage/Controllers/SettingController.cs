using Bilet11.DAL;
using Bilet11.Models;
using Bilet11.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bilet11.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="Admin")]
    public class SettingController : Controller
    {
        AppDbContext _context { get; }

        public SettingController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            PaginateVM<Setting> paginateVM = new PaginateVM<Setting>();
            paginateVM.CurrentPage = page;
            paginateVM.MaxPageCount = (int)Math.Ceiling((decimal)_context.Settings.Count() / 3);
            if (paginateVM.MaxPageCount == 0)
            {
                paginateVM.MaxPageCount = 1;
            }
            if (page < 1 || page > paginateVM.MaxPageCount) return BadRequest();
            paginateVM.Items = _context.Settings.Skip((page - 1) * 3).Take(3);
            return View(paginateVM);
        }
        public IActionResult Update(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Setting exist = _context.Settings.FirstOrDefault(p => p.Id == id);
            if (exist is null) return NotFound();
            UpdateSettingVM update = new UpdateSettingVM()
            {
                Key = exist.Key,
                Value = exist.Value
            };
            return View(update);
        }
        [HttpPost]
        public IActionResult Update(int? id, UpdateSettingVM update)
        {
            if (id is null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View();
            }
            Setting exist = _context.Settings.FirstOrDefault(e => e.Id == id);
            if (exist is null) return NotFound();
            exist.Value = update.Value;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
