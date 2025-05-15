    using System.ComponentModel.DataAnnotations;

    namespace WEBDEV.Models
    {
  
        public class Book
        {
        public int Id {  get; set; }
        public string Name { get; set; }   
        public string Author { get; set; }
        public int Mark { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        }

}
