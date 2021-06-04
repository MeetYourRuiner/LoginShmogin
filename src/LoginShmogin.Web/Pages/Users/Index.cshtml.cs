using System.Collections.Generic;
using System.Threading.Tasks;
using LoginShmogin.Infrastructure.Identity;
using LoginShmogin.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoginShmogin.Application.Interfaces;

namespace LoginShmogin.Web.Pages
{
    public class UsersModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IIdentityService _identityService;
        
        public IList<ApplicationUser> Users { get; set; }

        public UsersModel(AppDbContext context, IIdentityService identityService)
        {
            _identityService = identityService;
            _context = context;

        }
        public async Task OnGetAsync()
        {
            Users = await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<ActionResult> OnPostDeleteAsync(string id)
        {
            await _identityService.DeleteUserAsync(id);
            return RedirectToPage();
        }
    }
}
