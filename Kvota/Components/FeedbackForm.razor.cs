using System.ComponentModel.DataAnnotations;
using BlazorBootstrap;
using Kvota.Models;

namespace Kvota.Components
{
    partial class FeedbackForm
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

        private Feedback _feedback = new();
        private Modal? _modal;

        private string _subject = "Сообщение от клиента Квота";
         
        async void Submit()
        {
            var message =
                $"<div style=\"color: green;\">Сообщение от {_feedback.Name} </div><br /><div>Телефон: {_feedback.PhoneNumber} </div><br /><div>Email: " +
                $"{_feedback.Email} </div><br /><div>Парт номер: {_feedback.PartNumber} </div><br /><div>Комментарий: {_feedback.Comment} </div>";

                 await EmailSender.SendEmailAsync("dosaevk@yandex.ru",_subject,message);
            
                _modal?.ShowAsync();

        }

    }
}
