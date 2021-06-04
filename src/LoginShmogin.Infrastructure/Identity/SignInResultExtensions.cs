using System.Linq;
using LoginShmogin.Application.Models;
using Microsoft.AspNetCore.Identity;

namespace LoginShmogin.Infrastructure.Identity
{
    public static class SignInResultExtensions
    {
        public static Result ToApplicationResult(this SignInResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(new string[] {"Sign in failed"});
        }
    }
}