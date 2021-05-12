using System;
using LoginShmogin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using LoginShmogin.Infrastructure.Authentication.Identity;
using Microsoft.AspNetCore.Identity;

namespace LoginShmogin.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<AppDbContext>(options =>
				options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
				
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<AppDbContext>();
			return services;
		}
	}
}
