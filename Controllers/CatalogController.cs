using Microsoft.AspNetCore.Mvc;
using WEBDEV.Models;

public class CatalogController : Controller
{
    private readonly ApplicationDbContext _context;
    public CatalogController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        var books = _context.Books.ToList();

        return View(books);
    }

    public IActionResult Info(int id)
    {
        var book = _context.Books.FirstOrDefault(x => x.Id == id);
        if (book == null) {
            return NotFound();
        }
        return View(book);
    }
}
