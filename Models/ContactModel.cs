using System.ComponentModel.DataAnnotations;

namespace templateMVC.Models
{
    public class ContactModel
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        [Required]
        public string? Subject { get; set; }
        [Required]
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

}