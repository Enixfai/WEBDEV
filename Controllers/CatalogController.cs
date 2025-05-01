using Microsoft.AspNetCore.Mvc;
using WEBDEV.Models;

public class CatalogController : Controller
{
    public IActionResult Index()
    {
        var books = new List<Book>
        {
            new Book { Id = 1, Name = "1984", Author = "George Orwell", Mark = 8, Image = "/images/images.jpg"},
            new Book { Id = 2, Name = "Brave New World", Author = "Aldous Huxley", Mark = 8 , Image = "/images/images.jpg"}
        };

        return View(books);
    }
}
