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
        int? userId = HttpContext.Session.GetInt32("UserId");
        bool isBookmarked = false;

        if (userId != null)
        {
            isBookmarked = _context.Bookmarks.Any(bm => bm.BookId == id && bm.UserId == userId);
        }

        ViewBag.IsBookmarked = isBookmarked;
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


    public IActionResult Bookmarks()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Registration");
        }
        var bookmarks = _context.Bookmarks.Where(b => b.UserId == userId).Select(b => b.Book).ToList();
        return View(bookmarks);
    }
    

    [HttpPost]
    public IActionResult AddBookmark(int bookId)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Registration");
        }
        bool alreadyExists = _context.Bookmarks.Any(b => b.BookId == bookId && b.UserId == userId);
        if (!alreadyExists)
        {
            var bookmark = new Bookmark
            {
                UserId = userId.Value,
                BookId = bookId
            };
            _context.Bookmarks.Add(bookmark);
            _context.SaveChanges();
        }

        return RedirectToAction("Info", new { id = bookId });
    }
    [HttpPost]
    public IActionResult RemoveBookmark(int bookId)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Login", "Registration");
        }
        var bookmark = _context.Bookmarks.FirstOrDefault(bm => bm.BookId == bookId && bm.UserId == userId);
        if (bookmark != null)
        {
            _context.Bookmarks.Remove(bookmark);
            _context.SaveChanges();
        }
        return RedirectToAction("Info", new { id = bookId });
    }
}
