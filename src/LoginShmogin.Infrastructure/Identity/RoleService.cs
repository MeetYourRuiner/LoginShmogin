using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginShmogin.Application.DTOs;
using LoginShmogin.Application.Interfaces;
using LoginShmogin.Application.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoginShmogin.Infrastructure.Identity
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<Result> CreateRoleAsync(string name)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(name));
            return result.ToApplicationResult();
        }

        public async Task<Result> DeleteRoleByIdAsync(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            var result = await _roleManager.DeleteAsync(role);
            return result.ToApplicationResult();
        }

        public async Task<Result> DeleteRoleByNameAsync(string name)
        {
            IdentityRole role = await _roleManager.FindByNameAsync(name);
            var result = await _roleManager.DeleteAsync(role);
            return result.ToApplicationResult();
        }

        public async Task<IList<RoleDTO>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles
                .AsNoTracking()
                .Select((r) => new RoleDTO(r.Id, r.Name))
                .ToListAsync();
            return roles;
        }
    }
}