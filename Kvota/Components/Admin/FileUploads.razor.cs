using Kvota.Models.Products;
using Kvota.Repositories.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Kvota.Components.Admin
{
    partial class FileUploads
    {
        
            string Message = "не загружен файл";
            private IReadOnlyList<IBrowserFile> _selectedFiles = null!;

        [Parameter]
        public string? MyDirectory { get; set; }
        [Parameter]
        public string? Name { get; set; }
        [Parameter]
        public string? PatchImage { get; set; }
        private string? _directoryPath ;
        private string? _directoryP;
        [Parameter]
        public EventCallback<string> OnClickCallback { get; set; }
        protected override void OnInitialized()
        {
            _directoryPath = $"{env.WebRootPath}\\image\\{MyDirectory}\\{Name}";
            _directoryP = $"\\image\\{MyDirectory}\\{Name}";
        }
        private void OnInputFileChange(InputFileChangeEventArgs e)
        {
                _selectedFiles = e.GetMultipleFiles();
                Message = $"{_selectedFiles.Count} загружено";
                
        }

        private async void OnSubmit()
        {
            try
            {
                if (!Directory.Exists(_directoryPath))
                {
                    Directory.CreateDirectory(_directoryPath!);

                }
                

                foreach (var file in _selectedFiles)
                {
                    var stream = file.OpenReadStream(maxAllowedSize:1500000);
                    var path = $"{_directoryPath}\\{file.Name}";
                    var fs = File.Create(path);
                    await stream.CopyToAsync(fs);
                    stream.Close();
                    fs.Close();
                }
                PatchImage = _directoryP;
                await OnClickCallback.InvokeAsync(PatchImage);
                Message = $"загружено файлов {_selectedFiles.Count}";
               await InvokeAsync(StateHasChanged);
            }
            catch
            {
                // ignored
            }
        }

        private void DeleteImage(string path)
        {
            var fileInf = new FileInfo($"{env.WebRootPath}\\image\\{MyDirectory}\\{Name}\\{path}");
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }
        }
    }
    
}
