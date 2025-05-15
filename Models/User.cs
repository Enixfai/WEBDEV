    using System.ComponentModel.DataAnnotations;

    namespace WEBDEV.Models
    {
        public class User
        {
        
            public int id { get; set; }
            [Required(ErrorMessage ="Enter login")] 

            [StringLength(100, MinimumLength = 6, ErrorMessage = "Password should be between 6 and 100 characters.")]
            public string login { get; set; }

            [StringLength(100, MinimumLength = 6, ErrorMessage = "Password should be between 6 and 100 characters.")]
            [Required(ErrorMessage = "Enter password")]
            public string password { get; set; }
            
            public string image {  get; set; }


        }
    }
