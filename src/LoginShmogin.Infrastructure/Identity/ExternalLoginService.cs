using System.Collections.Generic;
using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using LoginShmogin.Application.Models;
using Microsoft.AspNetCore.Identity;

namespace LoginShmogin.Infrastructure.Identity
{
    public class ExternalLoginService : IExternalLoginService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExternalLoginService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IDictionary<string, string> CreateAuthenticationProperties(string provider, string redirectUrl)
        {
            var authParams = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return authParams.Items;
        }

        public async Task<ExternalServiceInfo> GetExternalServiceInfoAsync()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            return new ExternalServiceInfo() { ProviderName = info.ProviderDisplayName, Principal = info.Principal };
        }

        public async Task<Result> SignInAsync(bool isPersistent = false)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: isPersistent, bypassTwoFactor: true);
            return result.ToApplicationResult();
        }

        public async Task<Result> AddExternalLoginAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var info = await _signInManager.GetExternalLoginInfoAsync();
            var result = await _userManager.AddLoginAsync(user, info);
            return result.ToApplicationResult();
        }
    }
}