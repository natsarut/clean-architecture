﻿using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
