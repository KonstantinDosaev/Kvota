using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Kvota.Components.Admin
{
    partial class FileUploads
    {
        
            string Message = "No file(s) selected";
            IReadOnlyList<IBrowserFile> selectedFiles;

        [Parameter]
        public string? Directory { get; set; }
        [Parameter]
        public string? Name { get; set; }
        [Parameter]
        public string? PatchImage { get; set; }

        private void OnInputFileChange(InputFileChangeEventArgs e)
        {
                selectedFiles = e.GetMultipleFiles();
                Message = $"{selectedFiles.Count} file(s) selected";
                this.StateHasChanged();
        }

        private async void OnSubmit()
        {
            foreach (var file in selectedFiles)
            {
                Stream stream = file.OpenReadStream();
                //var path = $"{env.WebRootPath}\\{file.Name}";
                var path = $"{env.WebRootPath}\\image\\{Directory}\\{Name}.jpg";
                PatchImage = path;
                FileStream fs = File.Create(path);
                await stream.CopyToAsync(fs);
                stream.Close();
                fs.Close();
            }
            Message = $"загружено файлов {selectedFiles.Count}";
            this.StateHasChanged();
        }
    }
}
