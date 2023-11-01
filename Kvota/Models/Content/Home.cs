using Kvota.Interfaces;

namespace Kvota.Models.Content
{
   
    public class Home: IIdentifiable
    {
        public Guid Id { get; set; }

        public string? HomeImageTextOne { get; set; }

        public string? HomeImageTextTwo { get; set; }

        public string? PartnerTitle { get; set; }
        public string? AboutHomeText { get; set; }
        public string? AboutText { get; set; }
        public string? ProductTitle { get; set; }
        public bool CatalogView { get; set; }
        public bool BrandView { get; set; }
        public List<Guid> ProductInHome { get; set; }
    }
}
