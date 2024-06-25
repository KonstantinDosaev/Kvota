using BlazorBootstrap;
using Kvota.Constants;
using Kvota.Models.Content;


namespace Kvota.Pages.Admin
{
    partial class HomeTool
    {
        private Home Tools { get; set; }= default!;
  
        private Modal? _modal;
        private Modal? _modalImage;
        private string? DirectoryImage { get; set; }
        private string? NameImage { get; set; }
       
        protected override async Task OnInitializedAsync()
        {
            Tools = await HomeSerialize.DeSerialize($"{Links.RootPath}/{Links.HomeContentJson}");

        }


        private async  void SubmitPlayer()
        {
            await HomeSerialize.Serialize($"{Links.RootPath}/{Links.HomeContentJson}", Tools);
            _modal?.ShowAsync();

        }
        private async void ShowModalImage(string directory, string fileName)
        {
            DirectoryImage = directory;
            NameImage = fileName;
            await _modalImage!.ShowAsync();
            
        }

    }
}
