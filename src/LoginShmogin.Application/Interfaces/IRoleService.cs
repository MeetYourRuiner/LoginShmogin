using System.Collections.Generic;
using System.Threading.Tasks;
using LoginShmogin.Application.DTOs;
using LoginShmogin.Application.Models;

namespace LoginShmogin.Application.Interfaces
{
    public interface IRoleService
    {
        Task<Result> CreateRoleAsync(string name);
        Task<Result> DeleteRoleByIdAsync(string id);
        Task<Result> DeleteRoleByNameAsync(string name);
        Task<IList<RoleDTO>> GetRolesAsync();
    }
}