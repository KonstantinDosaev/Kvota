using Kvota.Models.UserAuth;
using Kvota.Services.Auth;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Kvota.Pages
{
    public partial class UserProfilePage
    {
        private MudForm? _userProfileForm;

        public KvotaUser? User { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var userName = await AuthService.CurrentUserInfo();
            User =  UserManager.Users.FirstOrDefault(f=>f.UserName==userName.UserName)!;
        }



        private async Task Submit()
        {
            if (_userProfileForm != null)
            {
                await _userProfileForm.Validate();

                if (_userProfileForm.IsValid)
                {
                    var userInfo = await ((CustomStateProvider)StateProvider).GetCurrentUser();
                    User.Email = User.UserName;
                    var t = await UserManager.UpdateAsync(User);

                    if (t != null && !User.UserName.Equals(userInfo.UserName))
                    {
                        await ((CustomStateProvider)StateProvider).UpdateAuthenticationStateAsync(new CurrentUser()
                        {
                            UserName = User.UserName,
                            Role = userInfo.Role
                        });
                        ImpNavigationManager.NavigateTo(ImpNavigationManager.Uri, forceLoad: true);
                    }
                }
            }
        }
    }
}
