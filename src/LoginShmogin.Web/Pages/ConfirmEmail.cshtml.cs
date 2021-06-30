using System.Text;
using System.Threading.Tasks;
using LoginShmogin.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace LoginShmogin.Web.Pages
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<ConfirmEmailModel> _logger;

        public ConfirmEmailModel(IIdentityService identityService, ILogger<ConfirmEmailModel> logger)
        {
            _logger = logger;
            _identityService = identityService;
        }

        [ViewData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _identityService.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                StatusMessage = "Thank you for confirming your email.";
                _logger.LogInformation("User with ID {UserId} confirmed his email", userId);
                await _identityService.AddToRoleAsync(userId, "User");
            }
            else
            {
                _logger.LogError("Error occured confirming email of user with ID {UserId}", userId);
                StatusMessage = "Error confirming your email.";
            }
            return Page();
        }
    }
}
