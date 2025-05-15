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
    [HttpPost]
    public async Task<IActionResult> UploadAvatar(IFormFile avatar)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null || avatar == null)
            return RedirectToAction("Index");

        var user = _context.Users.FirstOrDefault(u => u.id == userId);
        if (user == null)
            return NotFound();

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

}
