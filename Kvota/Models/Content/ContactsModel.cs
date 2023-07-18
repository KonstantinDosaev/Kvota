using Kvota.Interfaces;

namespace Kvota.Models.Content
{
    public class ContactsModel: IIdentifiable
    {
        public Guid Id { get; set; }

        public string? AddressOne { get; set; }
        public string? AddressTwo { get; set; }
        public string? PhoneOne { get; set; }
        public string? PhoneTwo { get; set; }
        public string? PhoneThree { get; set; }
        public string? EmailOne { get; set; }
        public string? EmailTwo { get; set; }
    }
}
