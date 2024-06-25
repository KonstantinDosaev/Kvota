using System.ComponentModel.DataAnnotations;

namespace Kvota.Models.UserAuth
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Введите e-mail!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Не является e-mail адресом")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите пароль!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите пароль повторно!")]
        [Compare(nameof(Password), ErrorMessage = "Пароль не совпадает!")]
        public string PasswordConfirm { get; set; }
    }
}
