using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoginShmogin.Application.DTOs;
using LoginShmogin.Application.Models;

namespace LoginShmogin.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<Result> CreateUserAsync(string email, string password);
        Task<Result> DeleteUserAsync(string userId);
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<Result> AddToRoleAsync(string userId, string role);
        Task<Result> ConfirmEmailAsync(string userId, string token);
        Task<Result> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
        Task<(string userId, string token)> GenerateEmailConfirmationTokenAsync(string email);
        Task<(string userId, string token)> GeneratePasswordResetTokenAsync(string email);
        Task<IList<UserDTO>> GetUsersAsync();
    }
}
