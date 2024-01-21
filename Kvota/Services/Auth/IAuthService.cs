using Kvota.Models.UserAuth;

namespace Kvota.Services.Auth
{
    public interface IAuthService
    {
        Task Login(LoginRequest loginRequest);
        Task Register(RegisterRequest registerRequest);
        Task Logout();
        Task<KvotaUser> CurrentUserInfo();

    }
}
