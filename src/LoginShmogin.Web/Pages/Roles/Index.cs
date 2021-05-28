using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LoginShmogin.Web.Pages
{
    public class RolesModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public IList<IdentityRole> Roles { get; set; }

        public RolesModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task OnGetAsync()
        {
            Roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
        }

        public async Task<ActionResult> OnPostDeleteAsync(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToPage();
        }
    }
}
