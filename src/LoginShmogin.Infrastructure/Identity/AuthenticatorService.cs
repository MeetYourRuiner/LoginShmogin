using System;
using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using LoginShmogin.Application.Models;
using Microsoft.AspNetCore.Identity;

namespace LoginShmogin.Infrastructure.Identity
{
    public class AuthenticatorService : IAuthenticatorService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticatorService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> GetAuthenticationUserId()
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            return user?.Id;
        }

        public async Task<Result> SignInAsync(string code, bool rememberMe, bool rememberMachine)
        {
            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(code, rememberMe, rememberMachine);
            return result.ToApplicationResult();
        }

        public async Task<string> GetAuthenticatorKeyAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.GetAuthenticatorKeyAsync(user);
        }

        public async Task<Result> ResetAuthenticatorKeyAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ResetAuthenticatorKeyAsync(user);
            return result.ToApplicationResult();
        }

        public async Task<bool> VerifyAuthenticatorCodeAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, code);
        }

        public async Task<Result> SetAuthenticatorEnabledAsync(string userId, bool state)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.SetTwoFactorEnabledAsync(user, state);
            return result.ToApplicationResult();
        }

        public async Task<bool> GetAuthenticatorEnabledAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.GetTwoFactorEnabledAsync(user);
        }

        public async Task<bool> IsMachineRememberedAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _signInManager.IsTwoFactorClientRememberedAsync(user);
        }

        public async Task ForgetMachineAsync()
        {
            await _signInManager.ForgetTwoFactorClientAsync();
        }
    }
}