using JobPortal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    [Route("[controller]/[action]")]
    public class CompanyController : Controller
    {
        
        [HttpGet] 
        [Authorize(Policy ="Company")]
        public async  Task<IActionResult> InsertJob(Jobs job) {
            return Json("Hi");
        }

    }
}
