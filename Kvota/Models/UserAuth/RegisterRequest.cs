using System.ComponentModel.DataAnnotations;

namespace Kvota.Models.UserAuth
{
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Пароль не совпадает!")]
        public string PasswordConfirm { get; set; }
    }
}
