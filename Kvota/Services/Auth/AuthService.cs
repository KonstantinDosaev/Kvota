using DocumentFormat.OpenXml.Spreadsheet;
using Kvota.Models.UserAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using DocumentFormat.OpenXml.Office2016.Excel;
using Kvota.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;


namespace Kvota.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<KvotaUser> _userManager;
        private readonly SignInManager<KvotaUser> _signInManager;
        private readonly AuthenticationStateProvider _authenticationStateProvider;


        public AuthService(UserManager<KvotaUser> userManager, SignInManager<KvotaUser> signInManager,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationStateProvider = authenticationStateProvider;

        }

        public async Task Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            var role = await _userManager.GetRolesAsync(user);
            //var claim = await _userManager.;
            if (user == null) throw new Exception("Пользователь не найден");
            if (!user.EmailConfirmed) throw new Exception("Подтверждение по электронной почте не пройдено");
            var singInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!singInResult.Succeeded) throw new Exception("Неверный пароль");

            var customAuthStateProvider = (CustomStateProvider)_authenticationStateProvider;
            await customAuthStateProvider.UpdateAuthenticationStateAsync(new CurrentUser()
            {
                UserName = user.UserName,
                Role = role.FirstOrDefault() ?? string.Empty
            });

        }

        public async Task Logout()
        {
            var customAuthStateProvider = (CustomStateProvider)_authenticationStateProvider;
            await customAuthStateProvider.UpdateAuthenticationStateAsync(null);
        }



        public async Task Register(RegisterRequest parameters)
        {
            var user = new KvotaUser
            {
                UserName = parameters.UserName,
                Email = parameters.UserName
            };
            var result = await _userManager.CreateAsync(user, parameters.Password);
            if (!result.Succeeded) throw new Exception(result.Errors.FirstOrDefault()?.Description);

            await _userManager.AddClaimsAsync(user, new List<Claim>
            {
                new Claim(ClaimTypes.Name, parameters.UserName),
                new Claim(ClaimTypes.Role, RoleNames.User),
            });

        }

        public async Task<KvotaUser> CurrentUserInfo()
        {

            var userAuth = await ((CustomStateProvider)_authenticationStateProvider).GetCurrentUser();
            if (userAuth == null)
                return null!;
            var user = await _userManager.FindByNameAsync(userAuth.UserName);
            return user;

        }

        



    }
}

