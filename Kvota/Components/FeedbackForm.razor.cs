using System.ComponentModel.DataAnnotations;
using BlazorBootstrap;
using Kvota.Models;

namespace Kvota.Components
{
    partial class FeedbackForm
    {
        private Feedback _feedback = new();
        private Modal? _modal;

        private string _subject = "Сообщение от клиента Квота";
         
        async void Submit()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();
            var _mailTo = config["EmailSettings:Sender"];
            var message =
                $"<div style=\"color: green;\">Сообщение от {_feedback.Name} </div><br /><div>Телефон: {_feedback.PhoneNumber} </div><br /><div>Email: " +
                $"{_feedback.Email} </div><br /><div>Парт номер: {_feedback.PartNumber} </div><br /><div>Комментарий: {_feedback.Comment} </div>";

            await EmailSender.SendEmailAsync(_mailTo, _subject, message);
            _modal?.ShowAsync();
            _feedback = new Feedback();


        }

    }
}
