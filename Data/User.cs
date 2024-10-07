using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace votek.Data
{
    public class User : IdentityUser
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string Gender { get; set; }
    }
}
