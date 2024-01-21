//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Identity;

//namespace Kvota.Pages
//{
//    partial class Administration
//    {
//        [CascadingParameter]
//        private Task<AuthenticationState> authenticationStateTask { get; set; } = null!;

//        string ADMINISTRATION_ROLE = "Administrators";
//        System.Security.Claims.ClaimsPrincipal CurrentUser = null!;
//        protected override async Task OnInitializedAsync()
//        {
//            var RoleResult = await _RoleManager.FindByNameAsync(ADMINISTRATION_ROLE);
//            if (RoleResult == null)
//            {
//                await _RoleManager.CreateAsync(new IdentityRole(ADMINISTRATION_ROLE));
//            }
//            var user = await _UserManager.FindByNameAsync("d@d");
//            if (user != null)
//            {
//                var UserResult = await _UserManager.IsInRoleAsync(user, ADMINISTRATION_ROLE);
//                if (!UserResult)
//                {
//                    await _UserManager.AddToRoleAsync(user, ADMINISTRATION_ROLE);
//                }
//            }
//            CurrentUser = (await authenticationStateTask).User;

//            GetUsers();
//            await InvokeAsync(StateHasChanged);
//        }

//        IdentityUser objUser = new IdentityUser();

//        string CurrentUserRole { get; set; } = "Users";

//        List<IdentityUser> ColUsers = new List<IdentityUser>();

//        List<string> Options = new List<string>() { "Users", "Administrators" };

//        string strError = "";

//        bool ShowPopup = false;

//        void AddNewUser()
//        {
//            objUser = new IdentityUser();
//            objUser.PasswordHash = "*****";
//            objUser.Id = "";
//            objUser.EmailConfirmed = true;
//            ShowPopup = true;
//        }
//        async Task SaveUser()
//        {
//            try
//            {
//                if (objUser.Id != "")
//                {
//                    var user = await _UserManager.FindByIdAsync(objUser.Id);
//                    user.Email = objUser.Email;
//                    user.EmailConfirmed= objUser.EmailConfirmed;
//                    await _UserManager.UpdateAsync(user);

//                    if (objUser.PasswordHash != "*****")
//                    {
//                        var resetToken = await _UserManager.GeneratePasswordResetTokenAsync(user);
//                        var passworduser = await _UserManager.ResetPasswordAsync(user, resetToken, objUser.PasswordHash);
//                        if (!passworduser.Succeeded)
//                        {
//                            if (passworduser.Errors.FirstOrDefault() != null)
//                            { strError = passworduser.Errors.FirstOrDefault()!.Description; }
//                            else
//                            { strError = "Pasword error"; }
//                            return;
//                        }
//                    }

//                    var UserResult = await _UserManager.IsInRoleAsync(user, ADMINISTRATION_ROLE);

//                    if ((CurrentUserRole == ADMINISTRATION_ROLE) & (!UserResult))
//                    {
//                        await _UserManager.AddToRoleAsync(user, ADMINISTRATION_ROLE);
//                    }
//                    else
//                    {
//                        if ((CurrentUserRole != ADMINISTRATION_ROLE) & (UserResult))
//                        {
//                            await _UserManager.RemoveFromRoleAsync(user, ADMINISTRATION_ROLE);
//                        }
//                    }
//                }
//                else
//                {
//                    var NewUser =
//                        new KvotaUser
//                        {
//                            UserName = objUser.UserName,
//                            Email = objUser.Email,
                            
//                        };
//                    var CreateResult = await _UserManager.CreateAsync(NewUser, objUser.PasswordHash);
//                    if (!CreateResult.Succeeded)
//                    {
//                        if (CreateResult.Errors.FirstOrDefault() != null)
//                        {
//                            strError = CreateResult.Errors.FirstOrDefault()!.Description;
//                        }
//                        else
//                        {
//                            strError = "Create error";
//                        }
//                    }
//                    else
//                    {
//                        if (CurrentUserRole == ADMINISTRATION_ROLE)
//                        {
//                            await _UserManager.AddToRoleAsync(NewUser, ADMINISTRATION_ROLE);
//                        }
//                    }
//                }
//                ShowPopup = false;
//                GetUsers();
//            }
//            catch (Exception ex)
//            {
//                strError = ex.GetBaseException().Message;
//            }
//        }
//        async Task EditUser(IdentityUser _IdentityUser)
//        {
//            objUser = _IdentityUser;

//            var user = await _UserManager.FindByIdAsync(objUser.Id);
//            if (user != null)
//            {
//                var UserResult =
//                    await _UserManager
//                        .IsInRoleAsync(user, ADMINISTRATION_ROLE);
//                if (UserResult)
//                {
//                    CurrentUserRole = ADMINISTRATION_ROLE;
//                }
//                else
//                {
//                    CurrentUserRole = "Users";
//                }
//            }
//            ShowPopup = true;
//        }
//        async Task DeleteUser()
//        {
//            ShowPopup = false;
//            var user = await _UserManager.FindByIdAsync(objUser.Id);
//            if (user != null)
//            {
//                await _UserManager.DeleteAsync(user);
//            }
//            GetUsers();
//        }
//        void ClosePopup()
//        {
//            // Close the Popup
//            ShowPopup = false;
//        }
//        public void GetUsers()
//        {
//            strError = "";
//            ColUsers = new List<IdentityUser>();

//            var user = _UserManager.Users.Select(x => new IdentityUser
//            {
//                Id = x.Id,
//                UserName = x.UserName,
//                Email = x.Email,
//                PasswordHash = "*****"
//            });
//            foreach (var item in user)
//            {
//                ColUsers.Add(item);
//            }
//        }
//    }
//}
