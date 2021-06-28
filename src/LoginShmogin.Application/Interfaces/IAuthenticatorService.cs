using System.Threading.Tasks;
using LoginShmogin.Application.Models;

namespace LoginShmogin.Application.Interfaces
{
    public interface IAuthenticatorService
    {
        Task<string> GetAuthenticationUserId();
        Task<string> GetAuthenticatorKeyAsync(string userId);
        Task<Result> ResetAuthenticatorKeyAsync(string userId);
        Task<Result> SetAuthenticatorEnabledAsync(string userId, bool state);
        Task<bool> GetAuthenticatorEnabledAsync(string userId);
        Task<Result> SignInAsync(string code, bool rememberMe, bool rememberMachine);
        Task<bool> VerifyAuthenticatorCodeAsync(string userId, string code);
        Task<bool> IsMachineRememberedAsync(string userId);
        Task ForgetMachineAsync();
    }
}