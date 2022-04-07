using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Eindproject_6v.Models;

namespace Eindproject_6v.Controllers;

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

    [Route("Explore")]
    public IActionResult Explore()
    {
        return View();
    }

    [Route("NotFound")]
    public IActionResult NotFound()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}