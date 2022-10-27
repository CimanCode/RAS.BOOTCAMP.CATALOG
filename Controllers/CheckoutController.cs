using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RAS.BOOTCAMP.TEMPLATE.Datas;
using RAS.BOOTCAMP.TEMPLATE.Models;
using RAS.BOOTCAMP.TEMPLATE.Datas.Entities;
using Microsoft.EntityFrameworkCore;

namespace RAS.Bootcamp.Catalog.Mvc.Net.Controllers;

public class CheckOutController : Controller
{
    private readonly ILogger<CheckOutController> _logger;
    private readonly KampDbContext _dbContext;

    public CheckOutController(ILogger<CheckOutController> logger, KampDbContext dbContext)
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
    
    [HttpPost]
    public IActionResult Order(){
         var user = int.Parse(User.Claims.First(y => y.Type == "ID").Value);
         var userid = _dbContext.Users.Include(e => e.Pembelies).First(e => e.IdUser == user);
         var pmbl = userid.Pembelies.First(x => x.IdUser == user);
         var orders = _dbContext.Keranjangs.Where(e => e.IdUser == user).ToList();
         var sum  = orders.Sum(e => e.Jumlah * e.HargaSatuan);

         var transaksi = new Transaksi();
         transaksi.IdUser = user;
         transaksi.TotalHarga = sum;
         transaksi.MetodePembayaran = "Cas On Delivery";
         transaksi.StatusTransaksi = "Menunggu Di Prosess";
         transaksi.StatusBayar = "Belum Di Bayar";
         transaksi.AlamatPengiriman = pmbl.Alamat;
        
        //untuk menambah data transaksi
        _dbContext.Transakses.Add(transaksi);
        _dbContext.SaveChanges();
        
        // var Ltr = _dbContext.Transakses.First(x => x.StatusTransaksi == "Waiting For Payment");

         foreach(var item in orders)
         {
            var itemTransaksi = new ItemTransaksi();
            itemTransaksi.IdBarang = item.IdBarang;
            itemTransaksi.Jumlah = item.Jumlah;
            itemTransaksi.Harga = item.HargaSatuan;
            itemTransaksi.SubTotal = itemTransaksi.Jumlah * itemTransaksi.Harga;
            // itemTransaksi.IdTransaksi = Ltr.IdTransaksi;

            transaksi.ItemTransaksis.Add(itemTransaksi);
         }

         //untuk menghapus data keranjang
         _dbContext.Keranjangs.RemoveRange(orders);

         _dbContext.SaveChanges();

         return RedirectToAction("Index", "Home");
    }

}
