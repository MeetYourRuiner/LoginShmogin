using System.Collections.Generic;
using System.Threading.Tasks;
using LoginShmogin.Application.DTOs;
using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LoginShmogin.Web.Pages
{
    public class RolesModel : PageModel
    {
        private readonly IRoleService _roleService;

        public IList<RoleDTO> Roles { get; set; }
        private readonly ILogger<RolesModel> _logger;

        public RolesModel(IRoleService roleService, ILogger<RolesModel> logger)
        {
            _logger = logger;
            _roleService = roleService;
        }

        public async Task OnGetAsync()
        {
            Roles = await _roleService.GetRolesAsync();
        }

        public async Task<ActionResult> OnPostDeleteAsync(string id)
        {
            var result = await _roleService.DeleteRoleByIdAsync(id);
            if (result.Succeeded)
            {
                _logger.LogInformation("Role with ID {RoleId} was deleted.", id);
            }
            else
            {
                _logger.LogError("Error occured, deleting Role with ID {RoleId}.", id);
            }
            return RedirectToPage();
        }
    }
}
