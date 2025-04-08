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
                return View("Success1");
            }
            ViewBag.ErrorMessage = "Неправильный логин или пароль.";
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
                return View("Success1");
            }
            return View("Index");
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
