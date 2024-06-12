using Kvota.Constants;
using Kvota.Models.Products;
using Kvota.Models.UserAuth;
using Kvota.Repositories.Products;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.OData.Query;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using static MudBlazor.CategoryTypes;
using System.Net.Http;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Kvota.Pages.Admin
{
    public partial class UsersTool
    {
        private MudTable<KvotaUser>? table;
        private List<KvotaUser> Elements = new List<KvotaUser>();
        private List<IdentityRole>? Roles { get; set; }
        private string _searchString = "";

        private HashSet<KvotaUser> selectedItems = new HashSet<KvotaUser>();
        private TableApplyButtonPosition applyButtonPosition = TableApplyButtonPosition.Start;
        private TableEditButtonPosition editButtonPosition = TableEditButtonPosition.Start;


        private KvotaUser _currentUser;
        private static List<string> _currentRoles;
        private DialogOptions _roleDialogOptions = new() { FullWidth = true, CloseButton = true };
        private bool _visibleRoleDialog;
        private string Value { get; set; } = "Nothing selected";
        private string? RoleFilterValue { get; set; }
        private int _totalItems;
        private bool getNotEmailConfirmed;
        private async Task OpenRoleDialog(KvotaUser user)
        {
            await LoadRoles();
            _currentUser = user;
            _currentRoles = (List<string>)await UserManager.GetRolesAsync(user);
            Value = _currentRoles.FirstOrDefault()!;
            _visibleRoleDialog = true;
        }

        protected override async Task OnInitializedAsync()
        {
            if (table != null) await table.ReloadServerData();
        }
        private async Task<TableData<KvotaUser>> ServerReload(TableState state)
        {
            IEnumerable<KvotaUser> data;
            if ((!string.IsNullOrEmpty(RoleFilterValue)||!string.IsNullOrWhiteSpace(RoleFilterValue)) && !getNotEmailConfirmed)
                data = await UserManager.GetUsersInRoleAsync(RoleFilterValue);
            else
                data = getNotEmailConfirmed ? UserManager.Users.IgnoreQueryFilters().Where(w=>!w.EmailConfirmed) : UserManager.Users;
            

            if (!string.IsNullOrEmpty(_searchString) || !string.IsNullOrWhiteSpace(_searchString))
            {
                data = data.Where(w =>
                    w.UserName != null && w.UserName.ToLower().Contains(_searchString.ToLower()) ||
                    w.CompanyName != null && w.CompanyName.ToLower().Contains(_searchString.ToLower()) ||
                    w.LastName != null && w.LastName.ToLower().Contains(_searchString.ToLower()));
            }
            _totalItems = data.Count();
            switch (state.SortLabel)
            {
                case "name_field":
                    data = data.OrderByDirection(state.SortDirection, o => o.UserName);
                    break;
                case "date_field":
                    data = data.OrderByDirection(state.SortDirection, o => o.DateTimeUpdate);
                    break; 
                case "employee_field":
                    data = data.OrderByDirection(state.SortDirection, o => o.LastName);
                    break; 
                case "company_field":
                    data = data.OrderByDirection(state.SortDirection, o => o.CompanyName);
                    break;
            }

           data = data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToList();
            return new TableData<KvotaUser>() { TotalItems = _totalItems, Items = data };
        }
       
        private void OnSearch(string text)
        {
            _searchString = text;
            table!.ReloadServerData();
        } 
        private void OnFilter(string text)
        {
            RoleFilterValue = text;
            table!.ReloadServerData();
        }
        private async Task GetNotEmailConfirmed()
        {
            getNotEmailConfirmed = getNotEmailConfirmed != true; 
            await table!.ReloadServerData();
        }
        private async Task LoadRoles()
        {
            Roles ??= await RoleManager.Roles.ToListAsync();
        }

        private bool FilterFunc(KvotaUser element)
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            if (element.Email.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.UserName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{element.Id}".Contains(_searchString))
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
            _currentUser.DateTimeUpdate = DateTime.UtcNow;
            var oldRole = await UserManager.GetRolesAsync(_currentUser);
            await UserManager.RemoveFromRolesAsync(_currentUser, oldRole);
            await UserManager.AddToRoleAsync(_currentUser, Value);
            _visibleRoleDialog = false;
        }
       
    }
}
