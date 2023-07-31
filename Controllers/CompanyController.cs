using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
