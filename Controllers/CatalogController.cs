
using System.Net.NetworkInformation;
using AspNetCoreGeneratedDocument;
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
    public IActionResult Read(int id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book == null) return NotFound();
        var filePath = Path.Combine("wwwroot", book.Text);
        var text = System.IO.File.ReadAllText(filePath);
        ViewBag.BookName = book.Name;
        return View("Read", text);
    }
}
