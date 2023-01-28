using Bilet11.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bilet11.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context { get; }

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Employees.Include(e=>e.Position));
        }
    }
}
