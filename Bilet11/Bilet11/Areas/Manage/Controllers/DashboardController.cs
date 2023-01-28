using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bilet11.Areas.Manage.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Manage")]
        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
