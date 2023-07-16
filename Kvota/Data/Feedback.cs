using MimeKit;
using System.ComponentModel.DataAnnotations;
using MailKit.Net.Smtp;

namespace Kvota.Data
{
    public class Feedback
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно быть не менее 2-х символов и не более 50-ти")]
        public string Name { get; set; }


        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public string PartNumber { get; set; }

        public string Comment { get; set; }


        public async Task<bool> SendEmailCustom(Feedback feedback)
        {
            try
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("Квота", "dosaevk@yandex.ru")); //отправитель сообщения
                message.To.Add(new MailboxAddress("dk", "dosaevk@yandex.ru")); //адресат сообщения
                message.Subject = "Сообщение от клиента Квота"; //тема сообщения
                message.Body = new BodyBuilder() { HtmlBody = $"<div style=\"color: green;\">Сообщение от {feedback.Name} </div><br /><div>Телефон: {feedback.PhoneNumber} </div><br /><div>Email: {feedback.Email} </div><br /><div>Парт номер: {feedback.PartNumber} </div><br /><div>Комментарий: {feedback.Comment} </div>" }.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync("smtp.yandex.ru", 587, false); //либо использум порт 465

                    await client.AuthenticateAsync("dosaevk@yandex.ru", "olhpmmoyrmywhypb"); //логин-пароль от аккаунта

                    await client.SendAsync(message);


                    await client.DisconnectAsync(true);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
