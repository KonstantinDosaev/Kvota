using System.ComponentModel.DataAnnotations;

namespace Kvota.Models.UserAuth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Введите e-mail!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Не является e-mail адресом")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите пароль!")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
