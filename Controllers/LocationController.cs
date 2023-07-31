using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    public class LocationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
