
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
    public IActionResult Index(string searchString)
    {
        var books = _context.Books.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            books = books.Where(b => b.Name.ToLower().Contains(searchString.ToLower()));
        }

        return View(books.ToList());
    }

    public IActionResult Search(string term)
    {
        var books = _context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(term))
            books = books.Where(b => b.Name.ToLower().Contains(term.ToLower()));

        return PartialView("_BookListPartial", books.ToList());
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
