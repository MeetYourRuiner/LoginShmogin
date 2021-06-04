using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using LoginShmogin.Application.Models;
using Microsoft.AspNetCore.Identity;

namespace LoginShmogin.Infrastructure.Identity
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
                return Result.Failure(new string[] {$"Unable to load user with ID '{userId}'." });
            var result = await _userManager.AddToRoleAsync(user, "User");
            return result.ToApplicationResult();
        }

        public async Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.ToApplicationResult();
        }

        public async Task<Result> ConfirmEmailAsync(string userId, string token)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.ToApplicationResult();
        }

        public async Task<Result> CreateUserAsync(string email, string password)
        {
            ApplicationUser newUser = new ApplicationUser
            {
                Email = email,
                UserName = email
            };
            var result = await _userManager.CreateAsync(newUser, password);
            return result.ToApplicationResult();
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.DeleteAsync(user);
            return result.ToApplicationResult();
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<Result> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.ToApplicationResult();
        }

        public async Task<(string userId, string token)> GenerateEmailConfirmationTokenAsync(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return (user.Id, token);
        }

        public async Task<(string userId, string token)> GeneratePasswordResetTokenAsync(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return (user.Id, token);
        }
    }
}