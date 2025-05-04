    using System.ComponentModel.DataAnnotations;

    namespace WEBDEV.Models
    {
    public record Book(int Id, string Name, string Author, int Mark, string Image, string Description);
}
