using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginShmogin.Infrastructure.Authentication.Identity;
using LoginShmogin.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LoginShmogin.Web.Pages
{
    public class UsersModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public IList<ApplicationUser> Users { get; set; }

        public UsersModel(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;

        }
        public async Task OnGetAsync()
        {
            Users = await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<ActionResult> OnPostDeleteAsync(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToPage();
        }
    }
}
