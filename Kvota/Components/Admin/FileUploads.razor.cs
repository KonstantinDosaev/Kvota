using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Kvota.Components.Admin
{
    partial class FileUploads
    {
        
            string Message = "не загружен файл";
            private IReadOnlyList<IBrowserFile> _selectedFiles = null!;

        [Parameter]
        public string? Directory { get; set; }
        [Parameter]
        public string? Name { get; set; }
        [Parameter]
        public string? PatchImage { get; set; }
        [Parameter]
        public EventCallback<string> OnClickCallback { get; set; }
        private void OnInputFileChange(InputFileChangeEventArgs e)
        {
                _selectedFiles = e.GetMultipleFiles();
                Message = $"{_selectedFiles.Count} загружено";
                this.StateHasChanged();
        }

        private async void OnSubmit()
        {
            foreach (var file in _selectedFiles)
            {
                
                Stream stream = file.OpenReadStream();
                //var path = $"{env.WebRootPath}\\{file.Name}";
                var path = $"{env.WebRootPath}\\image\\{Directory}\\{Name}.jpg";
                PatchImage = $"image\\{Directory}\\{Name}.jpg";
                FileStream fs = File.Create(path);
                await stream.CopyToAsync(fs);
                stream.Close();
                fs.Close();
                await OnClickCallback.InvokeAsync(PatchImage);
            }

            Message = $"загружено файлов {_selectedFiles.Count}";
            this.StateHasChanged();
        }

    }
}
