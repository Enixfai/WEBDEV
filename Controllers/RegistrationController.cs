using WEBDEV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WEBDEV.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();


        public RegistrationController(ApplicationDbContext context)
        {
            _context = context;
           
        }

        [HttpPost]
        public IActionResult Signin(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.login == login);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Wrong login or password";
                return View();
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.password, password);
            if (result == PasswordVerificationResult.Failed)
            {
                ViewBag.ErrorMessage = "Wrong login or password";
                return View();
            }
            HttpContext.Session.SetString("UserRole", user.role);
            HttpContext.Session.SetString("UserLogin", user.login);
            HttpContext.Session.SetInt32("UserId", user.id);
            HttpContext.Session.SetString("UserImage", user.image);
            return RedirectToAction("Index", "Catalog");
        }


        [HttpGet]
        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.login == user.login);

            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "User already exists";
                return View("Index");
            }
            user.password = _passwordHasher.HashPassword(user, user.password);
            user.image = "/avatars/default.png";
            user.role = "User";
            _context.Users.Add(user);
            _context.SaveChanges();
            HttpContext.Session.SetString("UserRole", user.role);
            HttpContext.Session.SetString("UserLogin", user.login);
            HttpContext.Session.SetInt32("UserId", user.id);
            HttpContext.Session.SetString("UserImage", user.image);
            return RedirectToAction("Index", "Catalog");
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
            HttpContext.Session.Clear();
            return View();
        }


        public IActionResult More()
        {
            return View();
        }
    }
}
