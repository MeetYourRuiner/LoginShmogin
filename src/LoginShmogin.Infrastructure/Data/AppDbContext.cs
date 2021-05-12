using System;
using LoginShmogin.Core.Entities;
using LoginShmogin.Infrastructure.Authentication.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoginShmogin.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //dotnet ef -s ../LoginShmogin.Web/
        }
    }
}
