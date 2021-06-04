using System.Security.Claims;
using System.Threading.Tasks;
using LoginShmogin.Application.Models;

namespace LoginShmogin.Application.Interfaces
{
    public interface ISignInService
    {
        bool IsSignedIn(ClaimsPrincipal claimsPrincipal);
        Task<Result> SignInAsync(string email, string password, bool rememberMe);
        Task SignOutAsync();
    }
}