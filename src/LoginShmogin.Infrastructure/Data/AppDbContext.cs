using System;
using LoginShmogin.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoginShmogin.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
		
        public DbSet<User> Users { get; set; }
    }
}
