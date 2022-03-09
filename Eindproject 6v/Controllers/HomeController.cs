﻿using Eindproject_6v.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject_6v.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("myportfolio")]
        public IActionResult MyPortfolio()
        {
            return View();
        }

        [Route("contact")]
        public IActionResult Contact()
        {           
            return View();
        }

        [HttpPost]
        [Route("contact")]
        public IActionResult Contact(Customer customer)
        {
            if (ModelState.IsValid)
                return Redirect("/succes");

            return View(customer);
        }

        [Route("aboutus")]
        public IActionResult Aboutus()
        {
            return View();
        }

        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("themes")]
        public IActionResult Themes()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
