using DocumentFormat.OpenXml.Spreadsheet;
using Kvota.Models.UserAuth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace Kvota.Pages.Admin
{
    public partial class UsersTool
    {
        private List<KvotaUser> Elements = new List<KvotaUser>();
        private List<IdentityRole> Roles = new List<IdentityRole>();

        private List<string> editEvents = new();
        private string searchString = "";
        private KvotaUser elementBeforeEdit;

        private HashSet<KvotaUser> selectedItems = new HashSet<KvotaUser>();
        private TableApplyButtonPosition applyButtonPosition = TableApplyButtonPosition.Start;
        private TableEditButtonPosition editButtonPosition = TableEditButtonPosition.Start;
        private TableEditTrigger editTrigger = TableEditTrigger.EditButton;


        private void OpenDialog() => _visibleRegisterDialog = true;
        private DialogOptions _registerDialogOptions = new() { FullWidth = true, CloseButton = true };
        private bool _visibleRegisterDialog;

        private KvotaUser _currentUser;
        private static List<string> _currentRoles;
        private DialogOptions _roleDialogOptions = new() { FullWidth = true, CloseButton = true };
        private bool _visibleRoleDialog;
        private string value { get; set; } = "Nothing selected";
        private async void OpenRoleDialog(KvotaUser user)
        {
            _currentUser = user;
            _currentRoles = (List<string>)await UserManager.GetRolesAsync(user);
            value = _currentRoles.FirstOrDefault()!;
            _visibleRoleDialog = true;
        }

        protected override async Task OnInitializedAsync()
        {

            Elements = await UserManager.Users.ToListAsync();
            Roles = await RoleManager.Roles.ToListAsync();


        }

        private void AddEditionEvent(string message)
        {
            editEvents.Add(message);
            StateHasChanged();
        }

        private void BackupItem(object element)
        {
            elementBeforeEdit = new()
            {
                Email = ((KvotaUser)element).Email,
                UserName = ((KvotaUser)element).UserName,
            };
            AddEditionEvent($"RowEditPreview event: made a backup of Element {((KvotaUser)element).UserName}");
        }

        private async void ItemHasBeenCommitted(object element)
        {
            elementBeforeEdit = new()
            {
                Email = ((KvotaUser)element).Email,
                UserName = ((KvotaUser)element).UserName,
            };
            await UserManager.UpdateAsync(elementBeforeEdit);
            AddEditionEvent($"RowEditCommit event: Changes to Element {((KvotaUser)element).UserName} committed");
        }

        private void ResetItemToOriginalValues(object element)
        {
            ((KvotaUser)element).Email = elementBeforeEdit.Email;
            ((KvotaUser)element).UserName = elementBeforeEdit.UserName;
            AddEditionEvent($"RowEditCancel event: Editing of Element {((KvotaUser)element).UserName} canceled");
        }

        private bool FilterFunc(KvotaUser element)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.UserName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{element.Id}".Contains(searchString))
                return true;
            return false;
        }

        [Inject] private IDialogService DialogService { get; set; }
        private async void OnButtonDeleteClicked()
        {
            bool? result = await DialogService.ShowMessageBox(
                "Внимание",
                "Подтвердите удаление!",
                yesText: "Удалить!  ", cancelText: "  Отменить");

            if (result != null && (bool)result)
            {
                RemoveUsers(selectedItems);
            }
            StateHasChanged();
        }
        private void RemoveUsers(HashSet<KvotaUser> selectedItems)
        {
            var tempList = new List<KvotaUser>(selectedItems);
            foreach (var user in tempList)
            {
                UserManager.DeleteAsync(user);
            }
            ImpNavigationManager.NavigateTo(ImpNavigationManager.Uri, forceLoad: true);
        }

        private async Task UpdateRoles()
        {
            var oldRole = await UserManager.GetRolesAsync(_currentUser);
            await UserManager.RemoveFromRolesAsync(_currentUser, oldRole);
            var result = await UserManager.AddToRoleAsync(_currentUser, value);
            _visibleRoleDialog = false;
        }
    }
}
