using System.ComponentModel.DataAnnotations;

namespace UklonBot.Models
{
    public class User
    {
        [Key]
        [Required]
        public string ViberId { get; set; }

        public string PhoneNumber { get; set; }

        public Cities City { get; set; }
    }
}