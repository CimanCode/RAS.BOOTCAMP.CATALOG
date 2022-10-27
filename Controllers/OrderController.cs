using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RAS.BOOTCAMP.TEMPLATE.Datas;
using RAS.BOOTCAMP.TEMPLATE.Models;
using RAS.BOOTCAMP.TEMPLATE.Datas.Entities;
using Microsoft.EntityFrameworkCore;

namespace RAS.Bootcamp.Catalog.Mvc.Net.Controllers;

public class OrderController : Controller
{
    private readonly ILogger<OrderController> _logger;
    private readonly KampDbContext _dbContext;

    public OrderController(ILogger<OrderController> logger, KampDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var user = int.Parse(User.Claims.First(y => y.Type == "ID").Value);
        List<Transaksi> tr = _dbContext.Transakses.Where(x => x.IdUser == user).ToList();
        return View(tr);
    }

    public IActionResult Detail(int id)
    {
        var transaksi = _dbContext.Transakses.First(x => x.IdTransaksi == id);
        var user = int.Parse(User.Claims.First(y => y.Type == "ID").Value);
        var pb = _dbContext.Pembelies.First(x => x.IdUser == user);
        return View(transaksi);
    }

    [HttpPost]
    public IActionResult OrderBuy(int id)
    {
        var transaksi = _dbContext.Transakses.First(x => x.IdTransaksi == id);
        transaksi.StatusTransaksi = "Wait Konfirmasi";
        transaksi.StatusBayar = "Telah di Bayar";
        _dbContext.SaveChanges();
         var user = int.Parse(User.Claims.First(y => y.Type == "ID").Value);
        List<Transaksi> Ltr = _dbContext.Transakses.Where(x => x.IdUser == user).ToList();
        return View("Index", Ltr);
    }

    [HttpPost]
    public IActionResult CancelBuy(int id)
    {
         var transaksi = _dbContext.Transakses.First(x => x.IdTransaksi == id);
        transaksi.StatusTransaksi = "Di Batalkan";
        transaksi.StatusBayar = "Di Batalkan";
        _dbContext.SaveChanges();
         var user = int.Parse(User.Claims.First(y => y.Type == "ID").Value);
        List<Transaksi> Ltr = _dbContext.Transakses.Where(x => x.IdUser == user).ToList();
        return View("Index", Ltr);
    }

    [HttpPost]
    public IActionResult Terima(int id)
    {
         var transaksi = _dbContext.Transakses.First(x => x.IdTransaksi == id);
        transaksi.StatusTransaksi = "Sudah Di Terima";
        _dbContext.SaveChanges();
         var user = int.Parse(User.Claims.First(y => y.Type == "ID").Value);
        List<Transaksi> Ltr = _dbContext.Transakses.Where(x => x.IdUser == user).ToList();
        return View("Index", Ltr);
    }

    [HttpPost]
    public IActionResult SelesaiTransaksi(int id)
    {
         var transaksi = _dbContext.Transakses.First(x => x.IdTransaksi == id);
        transaksi.StatusTransaksi = "Memesan Selesai";
        _dbContext.SaveChanges();
         var user = int.Parse(User.Claims.First(y => y.Type == "ID").Value);
        List<Transaksi> Ltr = _dbContext.Transakses.Where(x => x.IdUser == user).ToList();
        return View("Index", Ltr);
    }
}
