using System.Collections.Generic;
using System.Threading.Tasks;
using LoginShmogin.Application.Models;

namespace LoginShmogin.Application.Interfaces
{
    public interface IExternalLoginService
    {
        Task<Result> AddExternalLoginAsync(string userId);
        IDictionary<string, string> CreateAuthenticationProperties(string provider, string redirectUrl);
        Task<ExternalServiceInfo> GetExternalServiceInfoAsync();
        Task<Result> SignInAsync(bool isPersistent = false);
    }
}