using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WEBDEV.Models;

public class ProfileController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public ProfileController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        var user = _context.Users.FirstOrDefault(u => u.id == userId);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }
    [HttpPost]
    public async Task<IActionResult> UploadAvatar(IFormFile avatar)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null || avatar == null)
        {
            return RedirectToAction("Index");
        }
        var user = _context.Users.FirstOrDefault(u => u.id == userId);
        if (user == null)
        {
            return NotFound();
        }
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(avatar.FileName)}";
        var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars", fileName);

        using (var stream = new FileStream(savePath, FileMode.Create))
        {
            await avatar.CopyToAsync(stream);
        }

        user.image = "/avatars/" + fileName;
        _context.SaveChanges();

        HttpContext.Session.SetString("UserImage", user.image); 

        return RedirectToAction("Index");
    }
    [HttpPost]
    public IActionResult UpdateLogin(string newLogin)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Index");
        }
        var user = _context.Users.FirstOrDefault(u => u.id == userId);
        if (user == null)
        {
            return NotFound();
        }
        user.login = newLogin;
        _context.SaveChanges();

        HttpContext.Session.SetString("UserLogin", newLogin);

        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult ChangePass()
    {
        return View();
    }

    [HttpPost]
       public IActionResult ChangePass(ChangePassword model)
      {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Login", "Registration");

        var user = _context.Users.FirstOrDefault(u => u.id == userId);
        if (user == null)
            return NotFound();

        if (model.NewPassword != model.ConfirmPassword)
        {
            ViewBag.ErrorMessage = "Passwords don't match";
            return View(model);
        }

        var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.password, model.OldPassword);
        if (verifyResult == PasswordVerificationResult.Failed)
        {
            ViewBag.ErrorMessage = "Old password is incorrect";
            return View(model);
        }

        if (string.IsNullOrEmpty(model.NewPassword) || model.NewPassword.Length < 6)
        {
            ViewBag.ErrorMessage = "New password must contain at least 6 characters";
            return View(model);
        }

        user.password = _passwordHasher.HashPassword(user, model.NewPassword);
        _context.SaveChanges();

        ViewBag.SuccessMessage = "Password changed";
        return View(new ChangePassword());
       }

      }
