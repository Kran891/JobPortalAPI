﻿using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
