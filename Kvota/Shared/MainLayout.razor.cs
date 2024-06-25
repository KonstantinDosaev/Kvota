using Kvota.Models.Content;
using Microsoft.AspNetCore.Components;
using Kvota.Constants;

namespace Kvota.Shared
{
    partial class MainLayout
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }
        string UriHome { get; set; }
        Uri UriCurrent { get; set; }
        bool ViewTopBlock { get; set; }=true;
        private ContactsModel Contact { get; set; } = default!;
        
        protected override async Task OnInitializedAsync()
        {
           
            Links.RootPath = Env.WebRootPath;
            Contact = await ContactSerialize.DeSerialize($"{Links.RootPath}/{Links.ContactsJson}");
            UriCurrent = new Uri(NavigationManager.Uri);
            UriHome = UriCurrent.GetLeftPart(UriPartial.Authority) + "/";
            
            //ViewTopBlock = UriCurrent.ToString() != UriHome;
           

        }
        protected override async Task OnParametersSetAsync()
        {
            UriCurrent = new Uri(NavigationManager.Uri);
        
        }
        //protected override void OnAfterRender(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        UriCurrent = new Uri(NavigationManager.Uri);
        //       // ViewTopBlock = UriCurrent.ToString() != UriHome;
        //    }
        //    UriCurrent = new Uri(NavigationManager.Uri);

        //}

    }
}
