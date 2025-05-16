    using System.ComponentModel.DataAnnotations;

    namespace WEBDEV.Models
    {
  
        public class Book
        {
        public int Id {  get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public decimal Mark { get; set; }
        public string Image { get; set; }
        [Required]
        public string Description { get; set; }
        public string Text { get; set; }
        }

    }
