using System.Linq;
using LoginShmogin.Application.Models;
using Microsoft.AspNetCore.Identity;

namespace LoginShmogin.Infrastructure.Identity
{
    public static class SignInResultExtensions
    {
        public static Result ToApplicationResult(this SignInResult result)
        {
            if (result.Succeeded)
            {
                return Result.Success();
            }
            else if (result.RequiresTwoFactor)
            {
                return Result.TwoFactorRequired();
            }
            else if (result.IsLockedOut)
            {
                return Result.LockedOut();
            }
            else
            {
                return Result.Failure(new string[] { "Sign in failed" });
            }
        }
    }
}