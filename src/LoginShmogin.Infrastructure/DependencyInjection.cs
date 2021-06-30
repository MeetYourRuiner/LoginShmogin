using System;
using LoginShmogin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using LoginShmogin.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using LoginShmogin.Application.Interfaces;
using LoginShmogin.Infrastructure.Services;
using LoginShmogin.Application.Models;

namespace LoginShmogin.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            EmailSettings emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
            services.AddSingleton<IEmailSender, EmailSender>(s => new EmailSender(emailSettings));

            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(CustomTokenProviders.ResetAuthenticatorProvider);
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ISignInService, SignInService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IExternalLoginService, ExternalLoginService>();
            services.AddTransient<IAuthenticatorService, AuthenticatorService>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });
            services.ConfigureApplicationCookie(options =>
                {
                    // Cookie settings
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                    options.LoginPath = "/Login";
                    options.AccessDeniedPath = "/AccessDenied";
                    options.SlidingExpiration = true;
                });
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    GoogleSettings googleSettings =
                        configuration.GetSection("GoogleSettings").Get<GoogleSettings>();

                    options.ClientId = googleSettings.ClientId;
                    options.ClientSecret = googleSettings.ClientSecret;
                });
            return services;
        }
    }
}
