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
    public class ResetAuthenticatorModel : PageModel
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<ResetAuthenticatorModel> _logger;

        public ResetAuthenticatorModel(IIdentityService identityService, ILogger<ResetAuthenticatorModel> logger)
        {
            _logger = logger;
            _identityService = identityService;
        }
        public string Email { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (code == null || userId == null)
            {
                return BadRequest("A code must be supplied for authenticator reset.");
            }
            else
            {
                var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await _identityService.ResetAuthenticatorAsync(userId, token);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User with ID {UserId} reseted his authenticator", userId);
                    return RedirectToPage("/Login");
                }
                else
                {
                    _logger.LogError("Error occured resetting authenticator of user with Id {UserId}", userId);
                    return Page();
                }
            }
        }
    }
}
