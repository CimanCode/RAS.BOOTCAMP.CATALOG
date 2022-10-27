using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using RAS.BOOTCAMP.TEMPLATE.Datas;
using RAS.BOOTCAMP.TEMPLATE.Datas.Entities;

namespace RAS.Bootcamp.mvc.Controllers;
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly KampDbContext _dbContext;
     public AccountController(ILogger<AccountController> logger, KampDbContext dbContext)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    //Untuk tambah data pembeli
    [HttpGet]
    public IActionResult Login()
    {
        // List<User> = _dbContext.User.ToList();
        return View(new LoginRequest());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest br)
    {
        if(!ModelState.IsValid)
        {
        return View(br);
        }

        var user = _dbContext.Users.FirstOrDefault(X => X.Username == br.Username && X.Password == br.Password);

        if(user == null)
        {
            ViewBag.ErrorMessage = "password atawa usernamena salah bro";
            return View(br);
        }

        // if(user.tipe == "PEMBELI")
        // {
        //     ViewBag.ErrorMessage = "Maneh lain admin jeung lain tukang dagang";
        //     return View(br);
        // }

        //set autorization data to cookies
        var claims = new List<Claim>{
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("Fullname", user.Username),
            new Claim(ClaimTypes.Role, user.Tipe),
            new Claim("ID", user.IdUser.ToString()),
        };

        var calimsIdentity = new ClaimsIdentity
        (claims, CookieAuthenticationDefaults.AuthenticationScheme);
        
        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(calimsIdentity),
            authProperties);
        
        return RedirectToAction("Index", "Home");
    }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login");
        }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Register(RegisterRequest request)
    {
        if(!ModelState.IsValid)
        {
            return View(request);
        }

        var newUser = new BOOTCAMP.TEMPLATE.Datas.Entities.User        
        {
            Username = request.Username,
            Password = request.Password,
            Tipe = request.tipe,
        };

        var Penjual = new BOOTCAMP.TEMPLATE.Datas.Entities.Penjual
        {
            IdUser = newUser.IdUser,
            Alamat = request.Alamat,
            NamaToko = $"TK {request.FullName}",
            IdUserNavigation = newUser
        };

        var pembeli = new BOOTCAMP.TEMPLATE.Datas.Entities.Pembely
        {
            IdUser = newUser.IdUser,
            Nama = request.Nama,
            NoHandPhone = request.NoHandPhone,
            IdUserNavigation = newUser
        };
        

        _dbContext.Users.Add(newUser);
        _dbContext.Penjuals.Add(Penjual);

        _dbContext.SaveChanges();
        return RedirectToAction("Login");
    }

}