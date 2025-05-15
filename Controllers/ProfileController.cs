using Microsoft.AspNetCore.Mvc;
using WEBDEV.Models;

public class ProfileController : Controller
{
    private readonly ApplicationDbContext _context;

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
}
