using Microsoft.AspNetCore.Components;

namespace Kvota.Constants
{
    public static class Links
    {
        [Inject] public static IWebHostEnvironment Env { get; set; }


        public const string ContactsJson = "contactsContent.json";
        public const string HomeContentJson = "homeContent.json";
        public static string RootPath;
        public const string DefaultImageProduct = "image/products/default.jpg";
    }
}
