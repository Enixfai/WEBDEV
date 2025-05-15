using WEBDEV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WEBDEV.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ApplicationDbContext _context;
    
        
        public RegistrationController(ApplicationDbContext context)
        {
            _context = context;
           
        }

        [HttpPost]
        public IActionResult Signin(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.login == login && u.password == password);
            if (user!=null)
            {
                HttpContext.Session.SetString("UserLogin", user.login);
                HttpContext.Session.SetInt32("UserId", user.id);
                HttpContext.Session.SetString("UserImage", user.image);
                return RedirectToAction("Index", "Catalog");
            }
            ViewBag.ErrorMessage = "Wrong login or password";
            return View("Index");
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View("Index");
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.login == user.login);

            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Пользователь с таким логином уже существует. Выполните вход.";
                return View("Index");
            }
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                HttpContext.Session.SetString("UserLogin", user.login);
                HttpContext.Session.SetInt32("UserId", user.id);
                HttpContext.Session.SetString("UserImage", user.image);
                return RedirectToAction("Index", "Catalog");
            }
            return View("Index");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login", "Registration");
        }

        [HttpGet]
        public IActionResult Reg()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult More()
        {
            return View();
        }
    }
}
