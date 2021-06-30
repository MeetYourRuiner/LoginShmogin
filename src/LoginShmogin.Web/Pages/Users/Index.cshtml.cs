using System.Collections.Generic;
using System.Threading.Tasks;
using LoginShmogin.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginShmogin.Application.Interfaces;
using LoginShmogin.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace LoginShmogin.Web.Pages
{
    public class UsersModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IIdentityService _identityService;

        public IList<UserDTO> Users { get; set; }
        private readonly ILogger<UsersModel> _logger;

        public UsersModel(AppDbContext context, IIdentityService identityService, ILogger<UsersModel> logger)
        {
            _logger = logger;
            _identityService = identityService;
            _context = context;

        }
        public async Task OnGetAsync()
        {
            Users = await _identityService.GetUsersAsync();
        }

        public async Task<ActionResult> OnPostDeleteAsync(string id)
        {
            var result = await _identityService.DeleteUserAsync(id);
            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} was deleted.", id);
            }
            else
            {
                _logger.LogError("Error occured, deleting User with ID {UserId}.", id);
            }
            return RedirectToPage();
        }
    }
}
