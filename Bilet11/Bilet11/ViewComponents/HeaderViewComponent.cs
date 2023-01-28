using Bilet11.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Bilet11.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        AppDbContext _context { get;}

        public HeaderViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(_context.Settings.ToDictionary(s=>s.Key,s=>s.Value));
        }
    }
}
