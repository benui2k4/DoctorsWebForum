﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doctors_Web_Forum.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
