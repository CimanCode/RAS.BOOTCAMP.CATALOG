using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RAS.BOOTCAMP.TEMPLATE.Datas;
using RAS.BOOTCAMP.TEMPLATE.Models;
using RAS.BOOTCAMP.TEMPLATE.Datas.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RAS.Bootcamp.Catalog.Mvc.Net.Controllers;

[Authorize]
public class KeranjangController : Controller
{
    private readonly ILogger<KeranjangController> _logger;
    private readonly KampDbContext _dbContext;

    public KeranjangController(ILogger<KeranjangController> logger, KampDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var user = int.Parse(User.Claims.First(y => y.Type =="ID").Value);
        List<Keranjang>keranjang = _dbContext.Keranjangs.Include(x => x.IdBarangNavigation).ThenInclude(x => x.IdPenjualNavigation).Include(x => x.IdUserNavigation).Where(x => x.IdUser == user).ToList();
        return View(keranjang);
    }
    
    public IActionResult AddKeranjang(int id){
          Barang br = _dbContext.Barangs.First(e => e.IdBarang == id);
          int user = int.Parse(User.Claims.First(y => y.Type =="ID").Value);
          Keranjang kr = new Keranjang{
            IdBarang = id,
            IdUser = user,
            HargaSatuan = br.Harga, 
            Jumlah = 1
        };
        _dbContext.Keranjangs.Add(kr);
        _dbContext.SaveChanges();
        List<Keranjang> keranjangs = _dbContext.Keranjangs.ToList();
        return RedirectToAction("Index", "Keranjang", keranjangs);
    }

    [HttpPost]
    public IActionResult EditCart(int idbarang, int jumlah){
        Keranjang kr = _dbContext.Keranjangs.First(x => x.IdBarang == idbarang);
        kr.Jumlah = jumlah;
        _dbContext.SaveChanges();
        var user = int.Parse(User.Claims.First(y => y.Type == "ID").Value);
        List<Keranjang> ekr = _dbContext.Keranjangs.Include( i => i.IdBarangNavigation).ThenInclude(j => j.IdPenjualNavigation).Include(k => k.IdUserNavigation).Where(l => l.IdUser == user).ToList();
        return View("Index",ekr);
    }

    public IActionResult RemoveFromCart(int id){
        var datauser = int.Parse(User.Claims.First(e => e.Type == "ID").Value);
        Keranjang deleted = _dbContext.Keranjangs.First(e => e.IdBarang == id);
        _dbContext.Keranjangs.Remove(deleted);
        _dbContext.SaveChanges();
        List<Keranjang> ekr = _dbContext.Keranjangs.Include( i => i.IdBarangNavigation).ThenInclude(j => j.IdPenjualNavigation).Include(k => k.IdUserNavigation).Where(l => l.IdUser == datauser).ToList();
        return View("Index",ekr);
    }

    [HttpPost]
    public IActionResult Clear(){
        var datauser = int.Parse(User.Claims.First(e => e.Type == "ID").Value);
        List<Keranjang> delete = _dbContext.Keranjangs.Where(e => e.IdUser == datauser).ToList();
        _dbContext.Keranjangs.RemoveRange(delete);
        _dbContext.SaveChanges();
        List<Keranjang> ekr = _dbContext.Keranjangs.Include( i => i.IdBarangNavigation).ThenInclude(j => j.IdPenjualNavigation).Include(k => k.IdUserNavigation).Where(l => l.IdUser == datauser).ToList();
        return View("Index",ekr);
    }
}