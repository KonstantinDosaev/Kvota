using MimeKit;
using System.ComponentModel.DataAnnotations;
using MailKit.Net.Smtp;

namespace Kvota.Models
{
    public class Feedback
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно быть не менее 2-х символов и не более 50-ти")]
        public string? Name { get; set; }


        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? PartNumber { get; set; }

        public string? Comment { get; set; }


    }
}
