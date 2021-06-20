using System.Collections.Generic;
using System.Threading.Tasks;
using LoginShmogin.Application.Models;

namespace LoginShmogin.Application.Interfaces
{
    public interface IExternalLoginService
    {
        Task<Result> AddLoginAsync(string userId);
        Task<List<string>> GetLoginsAsync(string userId);
        IDictionary<string, string> CreateAuthenticationProperties(string provider, string redirectUrl);
        Task<ExternalLoginContext> GetCurrentLoginContextAsync();
        Task<List<string>> GetLoginProvidersAsync();
        Task<Result> SignInAsync(bool isPersistent = false);
        Task<Result> RemoveLoginAsync(string userId, string provider);
    }
}