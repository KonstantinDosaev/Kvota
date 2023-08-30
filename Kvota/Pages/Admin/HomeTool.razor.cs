using System.Text.Encodings.Web;
using System.Text.Json;
using BlazorBootstrap;
using Kvota.Constants;
using Kvota.Models.Content;
using Microsoft.AspNetCore.Components;


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
            await using var openStream = File.OpenRead(Links.HomeContentJson);
            Tools = (await JsonSerializer.DeserializeAsync<Home>(openStream))!;

        }


        private async  void SubmitPlayer()
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true, 
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
            };
            await using var createStream = File.Create(Links.HomeContentJson);
            await JsonSerializer.SerializeAsync(createStream, Tools, options);
            await createStream.DisposeAsync();
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
