    using System.ComponentModel.DataAnnotations;

    namespace WEBDEV.Models
    {
        public class User
        {
        
            public int id { get; set; }
            [Required(ErrorMessage ="Введите логин")]

            [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 100 символов.")]
            public string login { get; set; }

            [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 100 символов.")]
            [Required(ErrorMessage = "Введите пароль")]
            public string password { get; set; }

        }
    }
