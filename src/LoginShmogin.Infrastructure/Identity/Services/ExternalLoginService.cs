using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using LoginShmogin.Application.Models;
using LoginShmogin.Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.Identity;

namespace LoginShmogin.Infrastructure.Identity.Services
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

        public async Task<ExternalLoginContext> GetCurrentLoginContextAsync()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            return new ExternalLoginContext() { ProviderName = info.ProviderDisplayName, Principal = info.Principal };
        }

        public async Task<Result> SignInAsync(bool isPersistent = false)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: isPersistent, bypassTwoFactor: true);
            return result.ToApplicationResult();
        }

        public async Task<Result> AddLoginAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var info = await _signInManager.GetExternalLoginInfoAsync();
            var result = await _userManager.AddLoginAsync(user, info);
            return result.ToApplicationResult();
        }

        public async Task<Result> RemoveLoginAsync(string userId, string provider)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var logins = await _userManager.GetLoginsAsync(user);
            var loginToRemove = logins.Where(l => l.LoginProvider == provider).FirstOrDefault();
            var result = await _userManager.RemoveLoginAsync(user, loginToRemove.LoginProvider, loginToRemove.ProviderKey);
            return result.ToApplicationResult();
        }

        public async Task<List<string>> GetLoginsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var externalLogins = await _userManager.GetLoginsAsync(user);
            var externalLoginProvidersNames = externalLogins.Select(l => l.LoginProvider).ToList();
            return externalLoginProvidersNames;
        }

        public async Task<List<string>> GetLoginProvidersAsync()
        {
            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
            var providersNames = schemes.Select(s => s.DisplayName).ToList();
            return providersNames;
        }
    }
}