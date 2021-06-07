using System.Collections.Generic;
using System.Threading.Tasks;
using LoginShmogin.Application.DTOs;
using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoginShmogin.Web.Pages
{
    public class RolesModel : PageModel
    {
        private readonly IRoleService _roleService;

        public IList<RoleDTO> Roles { get; set; }

        public RolesModel(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task OnGetAsync()
        {
            Roles = await _roleService.GetRolesAsync();
        }

        public async Task<ActionResult> OnPostDeleteAsync(string id)
        {
            var result = await _roleService.DeleteRoleByIdAsync(id);
            return RedirectToPage();
        }
    }
}
