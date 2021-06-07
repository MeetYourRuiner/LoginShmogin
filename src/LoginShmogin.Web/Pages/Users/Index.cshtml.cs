using System.Collections.Generic;
using System.Threading.Tasks;
using LoginShmogin.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginShmogin.Application.Interfaces;
using LoginShmogin.Application.DTOs;

namespace LoginShmogin.Web.Pages
{
    public class UsersModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IIdentityService _identityService;
        
        public IList<UserDTO> Users { get; set; }

        public UsersModel(AppDbContext context, IIdentityService identityService)
        {
            _identityService = identityService;
            _context = context;

        }
        public async Task OnGetAsync()
        {
            Users = await _identityService.GetUsersAsync();
        }

        public async Task<ActionResult> OnPostDeleteAsync(string id)
        {
            await _identityService.DeleteUserAsync(id);
            return RedirectToPage();
        }
    }
}
