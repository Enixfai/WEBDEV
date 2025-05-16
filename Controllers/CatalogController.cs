using System.Net.NetworkInformation;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using WEBDEV.Models;
using Microsoft.EntityFrameworkCore;

public class CatalogController : Controller
{
    private readonly ApplicationDbContext _context;
    public CatalogController(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index(string searchString, string sortOrder)
    {
        var books = from b in _context.Books
                    select b;

        if (!string.IsNullOrEmpty(searchString))
        {
            books = books.Where(b => b.Name.ToLower().Contains(searchString.ToLower()));
        }
        switch (sortOrder)
        {
            case "name_desc":
                books = books.OrderByDescending(b => b.Name);
                break;
            case "mark_asc":
                books = books.OrderBy(b => b.Mark);
                break;
            case "mark_desc":
                books = books.OrderByDescending(b => b.Mark);
                break;
            default: 
                books = books.OrderBy(b => b.Name);
                break;
        }

        ViewBag.CurrentSort = sortOrder;
        return View(await books.ToListAsync());
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
        string userRole = HttpContext.Session.GetString("UserRole") ?? "";
        ViewBag.UserRole = userRole;
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

    public IActionResult AddBook()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AddBook(Book book, IFormFile imageFile, IFormFile textFile)
    {
        ModelState.Remove("Text");
        ModelState.Remove("Image");

        if (ModelState.IsValid && imageFile != null && textFile != null)
        {
            var imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var textFileName = Guid.NewGuid().ToString() + Path.GetExtension(textFile.FileName);

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageFileName);
            var textPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/text", textFileName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            using (var stream = new FileStream(textPath, FileMode.Create))
            {
                await textFile.CopyToAsync(stream);
            }

            book.Image = "/images/" + imageFileName;
            book.Text = "/text/" + textFileName;

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "The book has been added successfully!";
            return RedirectToAction("AddBook");
        }
       
        return View(book);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.Image.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        if (System.IO.File.Exists(imagePath))
        {
            System.IO.File.Delete(imagePath);
        }

        var textPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.Text.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        if (System.IO.File.Exists(textPath))
        {
            System.IO.File.Delete(textPath);
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Book was successfully deleted";
        return RedirectToAction("Index"); 
    }

}

