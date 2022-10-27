using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RAS.BOOTCAMP.TEMPLATE.Datas;
using RAS.BOOTCAMP.TEMPLATE.Datas.Entities;
using RAS.BOOTCAMP.TEMPLATE.Models;

namespace RAS.BOOTCAMP.TEMPLATE.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly KampDbContext _DbCOntext;
    public HomeController(ILogger<HomeController> logger, KampDbContext DbContext)
    {
        _DbCOntext = DbContext;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Barang> barangs = _DbCOntext.Barangs.ToList();
        return View(barangs);
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
