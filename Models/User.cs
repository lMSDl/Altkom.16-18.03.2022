using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class User : Entity
    {
        [Required]
        [StringLength(10, ErrorMessage = "Nazwa użytkownika musi mieć mniej niż 10 znaków")]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public Roles Roles { get; set; }

        public DateTime BithDate { get; set; }
    }
}
