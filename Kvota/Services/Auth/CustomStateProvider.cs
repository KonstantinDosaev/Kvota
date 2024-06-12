using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Kvota.Models.UserAuth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Kvota.Services.Auth
{
    public class CustomStateProvider : AuthenticationStateProvider
    {
        private CurrentUser _currentUser = null!;

        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        public CustomStateProvider( ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSessionStorageResult = await _sessionStorage.GetAsync<CurrentUser>("CurrentUser");
                var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;

                if (userSession == null)
                    return await Task.FromResult(new AuthenticationState(_anonymous));

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                     new Claim(ClaimTypes.Role, userSession.Role)
                }, "CustomAuth"));

                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        public async Task UpdateAuthenticationStateAsync(CurrentUser userSession)
        {
            ClaimsPrincipal claimsPrincipal;
            if (userSession != null)
            {
                await _sessionStorage.SetAsync("CurrentUser", userSession);


                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role, userSession.Role),
                }));

            }
            else
            {
                await _sessionStorage.DeleteAsync("CurrentUser");
                claimsPrincipal = _anonymous;
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task<CurrentUser> GetCurrentUser()
        {
            if (_currentUser != null ) return _currentUser;
            var userSessionStorageResult = await _sessionStorage.GetAsync<CurrentUser>("CurrentUser");
            var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
            if (userSession == null) return null;
                _currentUser = new CurrentUser()
                {
                    UserName = userSession.UserName,
                    Claims = userSession.Claims,
                    Role = userSession.Role
                };
            return _currentUser;
        }
    }
}
