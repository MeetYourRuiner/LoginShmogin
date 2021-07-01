using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginShmogin.Application.DTOs;
using LoginShmogin.Application.Interfaces;
using LoginShmogin.Application.Models;
using LoginShmogin.Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoginShmogin.Infrastructure.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> AddToRoleAsync(string userId, string role)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure(new string[] { $"Unable to load user with ID '{userId}'." });
            var result = await _userManager.AddToRoleAsync(user, "User");
            return result.ToApplicationResult();
        }

        public async Task<(string userId, Result result)> CreateUserAsync(string email, string password)
        {
            ApplicationUser newUser = new ApplicationUser
            {
                Email = email,
                UserName = email
            };
            string userId = string.Empty;
            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(email);
                userId = user?.Id;
            }
            return (userId, result.ToApplicationResult());
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.DeleteAsync(user);
            return result.ToApplicationResult();
        }

        public async Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.ToApplicationResult();
        }

        public async Task<(string userId, string token)> GenerateEmailConfirmationTokenAsync(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return (user.Id, token);
        }

        public async Task<Result> ConfirmEmailAsync(string userId, string token)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.ToApplicationResult();
        }

        public async Task<(string userId, string token)> GeneratePasswordResetTokenAsync(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return (user.Id, token);
        }

        public async Task<Result> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.ToApplicationResult();
        }

        public async Task<(string userId, string token)> GenerateAuthenticatorResetTokenAsync(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.GenerateUserTokenAsync(user, CustomTokenProviders.ResetAuthenticatorProvider, "ResetAuthenticator");
            return (user.Id, token);
        }

        public async Task<Result> ResetAuthenticatorAsync(string userId, string token)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            bool isTokenValid = await _userManager.VerifyUserTokenAsync(user, CustomTokenProviders.ResetAuthenticatorProvider, "ResetAuthenticator", token);
            if (isTokenValid)
            {
                var result = await _userManager.SetTwoFactorEnabledAsync(user, false);
                if (result.Succeeded)
                {
                    result = await _userManager.ResetAuthenticatorKeyAsync(user);
                    return result.ToApplicationResult();
                }
            }
            return Result.Failure(new string[] { "Token is invalid" });
        }
        public async Task<IList<UserDTO>> GetUsersAsync()
        {
            var users = await _userManager.Users
                .AsNoTracking()
                .Select((u) => new UserDTO(u.Id, u.UserName, u.Email, u.EmailConfirmed))
                .ToListAsync();
            return users;
        }

        public async Task<UserDTO> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return new UserDTO(user.Id, user.UserName, user.Email, user.EmailConfirmed);
        }

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return new UserDTO(user.Id, user.UserName, user.Email, user.EmailConfirmed);
        }
    }
}